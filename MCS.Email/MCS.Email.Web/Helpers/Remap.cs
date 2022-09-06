using AutoMapper;
using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Web.Contracts.Models;
using MimeKit;

namespace MCS.Email.Web.Helpers
{
    public class Remap: IRemap
    {
        private readonly IMapper mapper;
        public Remap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public EmailInfo RemapEmailWeb(EmailWeb? emailWeb)
        {
            var emailInfo = mapper.Map<EmailWeb, EmailInfo>(emailWeb);
            emailInfo.MailboxAddresses = new List<MailboxAddress>();
            foreach (var current in emailWeb?.Emails)
            {
                emailInfo.MailboxAddresses.Add(new MailboxAddress((current.Split('@'))[0], current));
            }
            return emailInfo;
        }
    }
}
