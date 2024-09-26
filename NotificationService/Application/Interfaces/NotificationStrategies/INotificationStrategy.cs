namespace NotificationService.Application.Interfaces.NotificationStrategies
{
    public interface INotificationStrategy
    {
        public Task Process(dynamic messageObj);
    }
}
