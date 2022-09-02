using MCS.Email.Domain.Contracts.Models;

namespace MCS.Email.Domain.Contracts.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailInfo emailInfo, string Id);
        Task SendEmailAsync(EmailInfo emailInfo, string Id);
    }
}
