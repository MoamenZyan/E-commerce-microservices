using NotificationService.Application.Interfaces.NotificationStrategies;
using Shared.Enums;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class NotificationStrategyContext
    {
        private readonly IServiceProvider _serviceProvider;
        public NotificationStrategyContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INotificationStrategy? GetStrategy(MessageTypes messageType)
        {
            return messageType switch
            {
                MessageTypes.Welcome => _serviceProvider.GetService<IWelcomeNotificationStrategy>(),
                MessageTypes.ConfirmEmail => _serviceProvider.GetService<IConfirmEmailNotificationStrategy>(),
                MessageTypes.ResetPassword => _serviceProvider.GetService<IResetPasswordStrategy>(),
                _ => null
            };
        }
    }
}
