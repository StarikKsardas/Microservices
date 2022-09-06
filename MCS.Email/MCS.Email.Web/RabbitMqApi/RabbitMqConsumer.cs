using AutoMapper;
using FluentValidation;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Domain.Contracts.Services;
using MCS.Email.Web.Contracts.Configurations;
using MCS.Email.Web.Contracts.Models;
using MCS.Email.Web.Helpers;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MCS.Email.Web.RabbitMqApi
{
    public class RabbitMqConsumer : BackgroundService, IRabbitMqConsumer
    {
        private readonly IEmailService emailService;
        private readonly ILogger<RabbitMqConsumer> logger;
        private readonly IRemap remap;
        private readonly AbstractValidator<EmailWeb> validator;
        private readonly IRabbitMqProducer producer;
        private readonly EmailRabbitConfiguration configuration;
        private readonly IHttpContextAccessor httpContext;

        private readonly IConnection connection;
        private readonly IModel channel;
        public RabbitMqConsumer(IEmailService emailService, ILogger<RabbitMqConsumer> logger,
            IRemap remap, AbstractValidator<EmailWeb> validator, IRabbitMqProducer producer, EmailRabbitConfiguration configuration, IHttpContextAccessor httpContext)
        {
            this.emailService = emailService;
            this.logger = logger;
            this.remap = remap;
            this.validator = validator;
            this.producer = producer;
            this.configuration = configuration;
            this.httpContext = httpContext;

            var factory = new ConnectionFactory()
            {
                HostName = configuration.HostName,
                Port = configuration.Port,
                UserName = configuration.Login,
                Password = configuration.Password
            };
            
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange: configuration.ExchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: configuration.ConsumerQueue, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: configuration.ConsumerQueue, exchange: configuration.ExchangeName, routingKey: configuration.ConsumerRoutingKey);
            this.httpContext = httpContext; 
        }

        private string GetValidationErrors(EmailWeb emailWeb)
        {
            var result = "";
            var validationResult = validator.Validate(emailWeb);
            if (!validationResult.IsValid)
            {
                result = string.Join(Environment.NewLine, validationResult.Errors);
            }
            return result;
        }

        public void Send()
        {
            EmailWeb? emailWeb = null;
            var rabbitEmailResult = new RabbitEmailResult();

            var consumer = new EventingBasicConsumer(channel);            
            consumer.Received += (sender, e) =>
            {
                var body = e.Body;
                logger.LogInformation("Received Message");
                var message = Encoding.UTF8.GetString(body.ToArray());
                emailWeb = JsonConvert.DeserializeObject<EmailWeb>(message);
                rabbitEmailResult.Id = emailWeb?.Id;
                httpContext?.HttpContext?.Items.Add("Id_Message", emailWeb?.Id);                       
                logger.LogInformation($"Received Message {emailWeb?.Id}");

                var errors = GetValidationErrors(emailWeb);
                channel.BasicAck(e.DeliveryTag, false);
                if (string.IsNullOrEmpty(errors))
                {
                    try
                    {
                        var emailInfo = remap.RemapEmailWeb(emailWeb);
                        emailService.SendEmail(emailInfo, emailWeb?.Id);
                        rabbitEmailResult.IsSend = true;
                        rabbitEmailResult.IsValid = true;
                        rabbitEmailResult.Message = errors;
                        logger.LogInformation(rabbitEmailResult.ToString());
                    }
                    catch(Exception exception)
                    {                        
                        rabbitEmailResult.IsValid = true;
                        rabbitEmailResult.IsSend = false;
                        rabbitEmailResult.Message = exception.ToString();
                        producer.SendEmailResultMessage(rabbitEmailResult);
                        logger.LogError(rabbitEmailResult.ToString());
                        throw;
                    }                    
                }
                else
                {
                    rabbitEmailResult.IsSend = false;
                    rabbitEmailResult.IsValid = false;
                    rabbitEmailResult.Message = errors;
                    logger.LogError(rabbitEmailResult.ToString());
                }
                producer.SendEmailResultMessage(rabbitEmailResult);
            };
            channel.BasicConsume(configuration.ConsumerQueue, autoAck: false, consumer: consumer);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();            
            Send();
            return Task.CompletedTask;
        }

       
    }
}
