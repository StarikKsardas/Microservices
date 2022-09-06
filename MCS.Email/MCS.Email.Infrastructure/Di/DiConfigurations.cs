using MCS.Email.Domain.Services.Configurations;
using MCS.Email.Web.Contracts.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Infrastructure.Di
{
    public static class DiConfigurations
    {
        public static IServiceCollection AddEmailServerConfigurations(this IServiceCollection services)
        {
            var emailConfiguration = new EmailConfiguration()
            {
                From = Environment.GetEnvironmentVariable("FROM"),
                ViewName = Environment.GetEnvironmentVariable("VIEWNAME"),
                IsSSL = bool.Parse(Environment.GetEnvironmentVariable("ISSSL") ?? "false"),
                SmtpServer = Environment.GetEnvironmentVariable("SMTPSERVER"),
                Port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "80"),
                UserName = Environment.GetEnvironmentVariable("USERNAME"),
                Password = Environment.GetEnvironmentVariable("PASSWORD"),
            };
            
            services.AddSingleton(emailConfiguration);
            return services;
        }

        public static IServiceCollection AddEmailRabbitConfiguration(this IServiceCollection services)
        {
            var emaiRabbitConfiguration = new EmailRabbitConfiguration()
            {
                IsUse = bool.Parse(Environment.GetEnvironmentVariable("ISUSERABBITMQ") ?? "false"),
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"),
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                Login = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"),
                ExchangeName = Environment.GetEnvironmentVariable("RABBITMQ_EXCANGE_NAME"),
                ProducerRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCER_ROUTINGKEY"),
                ConsumerRoutingKey = Environment.GetEnvironmentVariable("RABBITMQ_CONSUMER_ROUTINGKEY"),
                ProducerQueue = Environment.GetEnvironmentVariable("RABBITMQ_PRODUCER_QUEUE"),
                ConsumerQueue = Environment.GetEnvironmentVariable("RABBITMQ_CONSUMER_QUEUE")
            };        

            services.AddSingleton(emaiRabbitConfiguration);
            return services;
        }
    }
}
