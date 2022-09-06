using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Web.Contracts.Configurations
{
    public class EmailRabbitConfiguration
    {
        public bool IsUse { get; set; }
        public string? HostName { get; set; }
        public int Port { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? ExchangeName { get; set; }
        public string? ProducerRoutingKey { get; set; }
        public string? ConsumerRoutingKey { get; set; } 
        public string? ProducerQueue { get; set; }
        public string? ConsumerQueue { get; set; }    
    }
}
