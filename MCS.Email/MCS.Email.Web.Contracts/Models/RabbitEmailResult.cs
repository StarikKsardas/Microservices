using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Web.Contracts.Models
{
    public class RabbitEmailResult
    {
        public string? Id { get; set; }
        public bool IsValid { get; set; }
        public bool IsSend { get; set; }
        public string? Message { get; set; }
       
        public override string ToString()
        {
            return $"Id: {Id}, IsValid: {IsValid}, IsSend: {IsSend}, Message:{Message}";
        }
    }
}
