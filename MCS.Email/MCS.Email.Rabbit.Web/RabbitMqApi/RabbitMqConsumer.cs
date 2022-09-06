using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MCS.Email.Rabbit.Web.RabbitMqApi
{
    public class RabbitMqConsumer : BackgroundService, IRabbitMqConsumer
    {
        public RabbitMqConsumer()
        {
        }

        public void GetEmailData()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "email", type: ExchangeType.Direct);
                    var queueName = channel.QueueDeclare(exclusive: false, autoDelete:false).QueueName;
                    channel.QueueBind(queue: queueName, exchange: "email", routingKey: "email2send");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());                        
                    };
                    channel.BasicConsume(queueName, autoAck: true, consumer: consumer);
                }
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();


            return Task.CompletedTask;
        }
    }
}
