namespace MCS.Email.Web.RabbitMqApi
{
    public interface IRabbitMqProducer
    {
        public void SendEmailResultMessage<T>(T Message);
    }
}
