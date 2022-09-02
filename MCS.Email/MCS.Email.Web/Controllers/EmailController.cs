using AutoMapper;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Domain.Contracts.Services;
using MCS.Email.Web.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace MCS.Email.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> logger;
        private readonly IEmailService emailService;
        private readonly IMapper mapper;

        public EmailController(ILogger<EmailController> logger, IEmailService emailService, IMapper mapper)
        { 
            this.logger = logger;
            this.emailService = emailService;
            this.mapper = mapper;
        }

        [HttpPost("Send")]
        public IActionResult Send([FromBody]EmailWeb emailWeb)
        {
            try
            {
                var emailInfo = mapper.Map<EmailWeb, EmailInfo>(emailWeb);
                emailInfo.MailboxAddresses = new List<MailboxAddress>();
                foreach (var current in emailWeb.Emails)
                {
                    emailInfo.MailboxAddresses.Add(new MailboxAddress((current.Split('@'))[0], current));
                }
                emailService.SendEmail(emailInfo, emailWeb.Id);
                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("SendAsync")]
        public async Task<IActionResult> SendAsync([FromBody] EmailWeb emailWeb)
        {
            try
            {
                var emailInfo = mapper.Map<EmailWeb, EmailInfo>(emailWeb);
                emailInfo.MailboxAddresses = new List<MailboxAddress>();
                foreach (var current in emailWeb.Emails)
                {
                    emailInfo.MailboxAddresses.Add(new MailboxAddress((current.Split('@'))[0], current));
                }
                await emailService.SendEmailAsync(emailInfo, emailWeb.Id);
                return Ok();
            }
            catch
            {
                throw;
            }
        }




    }
}
