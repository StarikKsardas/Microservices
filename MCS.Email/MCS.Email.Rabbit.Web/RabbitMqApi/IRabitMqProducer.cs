namespace MCS.Email.Rabbit.Web.RabbitMqApi
{
    public interface IRabitMqProducer
    {
        public void SendEmailResultMessage<T>(T Message);
    }
}
