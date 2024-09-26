namespace NotificationService.Application.EmailStrategies
{
    public interface IEmailStrategy
    {
        public Task Send(string userName, string email, string body);
    }
}
