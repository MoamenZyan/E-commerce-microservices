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
                MessageTypes.Welcome => _serviceProvider.GetRequiredService<IWelcomeNotificationStrategy>(),
                MessageTypes.ConfirmEmail => _serviceProvider.GetRequiredService<IConfirmEmailNotificationStrategy>(),
                MessageTypes.ResetPassword => _serviceProvider.GetRequiredService<IResetPasswordStrategy>(),
                MessageTypes.ProductAddedToCart => _serviceProvider.GetRequiredService<IProductAddedToCartNotificationStrategy>(),
                MessageTypes.ProductCreated => _serviceProvider.GetRequiredService<IProductCreatedNotificationStrategy>(),
                MessageTypes.OrderConfirmed => _serviceProvider.GetRequiredService<IOrderConfirmedNotificationStrategy>(),
                _ => null
            };
        }
    }
}
