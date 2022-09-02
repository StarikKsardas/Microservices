using MCS.Email.Domain.Services.Configurations;
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
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
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
    }
}
