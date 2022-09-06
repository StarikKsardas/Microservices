using AutoMapper;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Domain.Contracts.Services;
using MCS.Email.Web.Contracts.Models;
using MCS.Email.Web.Helpers;
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
        private readonly IRemap remap;

        public EmailController(ILogger<EmailController> logger, IEmailService emailService, IRemap remap)
        { 
            this.logger = logger;
            this.emailService = emailService;
            this.remap = remap;
        }

        [HttpPost("Send")]
        public IActionResult Send([FromBody]EmailWeb emailWeb)
        {
            HttpContext.Items.Add("Id_Message", emailWeb.Id);
            try
            {
                var emailInfo = remap.RemapEmailWeb(emailWeb);
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
            HttpContext.Items.Add("Id_Message", emailWeb.Id);
            try
            {
                var emailInfo = remap.RemapEmailWeb(emailWeb);
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
