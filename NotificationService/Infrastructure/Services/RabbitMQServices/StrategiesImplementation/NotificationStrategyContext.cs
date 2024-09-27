using NotificationService.Application.Interfaces.NotificationStrategies;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class NotificationStrategyContext
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationStrategyContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotificationStrategy? GetStrategy(string messageType)
        {
            return messageType switch
            {
                "welcome" => _serviceProvider.GetService<IWelcomeNotificationStrategy>(),
                "confirmEmail" => _serviceProvider.GetService<IConfirmEmailNotificationStrategy>(),
                "passwordReset" => _serviceProvider.GetService<IResetPasswordStrategy>(),
                _ => null
            };
        }
    }
}
