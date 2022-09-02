using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Web.Contracts.Models
{
    public class EmailWeb
    {
        public string? Id { get; set; }
        public List<string>? Emails { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}
