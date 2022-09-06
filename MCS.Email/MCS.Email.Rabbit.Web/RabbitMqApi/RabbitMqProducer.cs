using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MCS.Email.Rabbit.Web.RabbitMqApi
{
    public class RabbitMqProducer : IRabitMqProducer
    {
        private readonly ILogger<RabbitMqProducer> logger;

        public RabbitMqProducer(ILogger<RabbitMqProducer> logger)
        {
            this.logger = logger;
        }

        public void SendEmailResultMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "email", type: ExchangeType.Direct);
                    var json = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    channel.BasicPublish(exchange: "email", routingKey: "emailresult", body: body);
                }
            }
        }
    }
}
