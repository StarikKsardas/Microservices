using MCS.Email.Web.Contracts.Configurations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MCS.Email.Web.RabbitMqApi
{
    public class RabbitMqProducer : IRabbitMqProducer
    {
        private readonly ILogger<RabbitMqProducer> logger;
        private readonly EmailRabbitConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;  

        public RabbitMqProducer(ILogger<RabbitMqProducer> logger, EmailRabbitConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;            
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void SendEmailResultMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                Port = configuration.Port,
                UserName = configuration.Login,
                Password = configuration.Password
            };
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {                    
                    channel.ExchangeDeclare(exchange: configuration.ExchangeName, type: ExchangeType.Direct);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.QueueDeclare(queue: configuration.ProducerQueue, exclusive: false, autoDelete: false);
                    channel.QueueBind(queue: configuration.ProducerQueue, exchange: configuration.ExchangeName, routingKey: configuration.ProducerRoutingKey);
                    channel.BasicPublish(exchange: configuration.ExchangeName, routingKey: configuration.ProducerRoutingKey, body: body);
                    logger.LogInformation($"Email with id {httpContextAccessor?.HttpContext?.Items["Id_Message"]?.ToString()} was sent correct");
                }
            }
        }
    }
}
