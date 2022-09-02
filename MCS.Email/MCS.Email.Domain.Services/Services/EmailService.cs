using MailKit.Net.Smtp;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Domain.Contracts.Services;
using MCS.Email.Domain.Services.Configurations;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Domain.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> logger;
        private readonly EmailConfiguration emailConfiguration;

        public EmailService(ILogger<EmailService> logger, EmailConfiguration emailConfiguration)
        {
            this.logger = logger;
            this.emailConfiguration = emailConfiguration;
        }

        public void SendEmail(EmailInfo emailInfo, string Id)
        {
            logger.LogInformation($"Message {Id} start to be sent", emailInfo.MailboxAddresses);
            var mimeMessage = CreateEmailMessage(emailInfo);
            Send(mimeMessage);
            logger.LogInformation($"Message {Id} finish to be sent", emailInfo.MailboxAddresses);
        }

        public async Task SendEmailAsync(EmailInfo emailInfo, string Id)
        {
            logger.LogInformation($"Message {Id} start to be sent", emailInfo.MailboxAddresses);
            var mimeMessage = CreateEmailMessage(emailInfo);
            await SendAsync(mimeMessage);
            logger.LogInformation($"Message {Id} finish to be sent", emailInfo.MailboxAddresses);
        }

        private MimeMessage CreateEmailMessage(EmailInfo emailInfo)
        {
            var result = new MimeMessage();
            result.From.Add(new MailboxAddress(emailConfiguration.ViewName, emailConfiguration.From));
            result.To.AddRange(emailInfo.MailboxAddresses);
            result.Subject = emailInfo.Subject;
            result.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = emailInfo.Content };
            return result;
        }

        private void Send(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(emailConfiguration.SmtpServer, emailConfiguration.Port, emailConfiguration.IsSSL);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailConfiguration.UserName, emailConfiguration.Password);
                    client.Send(mimeMessage);
                }
                catch
                {                    
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mimeMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, emailConfiguration.IsSSL);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);
                    await client.SendAsync(mimeMessage);
                }
                catch
                {                   
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
