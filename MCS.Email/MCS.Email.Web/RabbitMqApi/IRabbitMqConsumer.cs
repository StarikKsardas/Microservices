using MCS.Email.Web.Contracts.Models;

namespace MCS.Email.Web.RabbitMqApi
{
    public interface IRabbitMqConsumer: IHostedService
    {
        public void Send();
    }
}
