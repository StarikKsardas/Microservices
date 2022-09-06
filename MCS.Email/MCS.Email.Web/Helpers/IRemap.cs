using MCS.Email.Domain.Contracts.Models;
using MCS.Email.Web.Contracts.Models;

namespace MCS.Email.Web.Helpers
{
    public interface IRemap
    {
        EmailInfo RemapEmailWeb(EmailWeb? emailWeb);
    }
}
