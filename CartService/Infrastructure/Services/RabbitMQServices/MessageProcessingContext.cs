using CartService.Infrastructure.Services.RabbitMQServices.MessageProcessingStrategies;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enums;

namespace CartService.Infrastructure.Services.RabbitMQServices
{
    public class MessageProcessingContext
    {
        private readonly IServiceProvider _serviceProvider;
        public MessageProcessingContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessageProcessingStrategy? GetStrategy(MessageTypes type)
        {
            return type switch
            {
                MessageTypes.ProductDeleted => _serviceProvider.GetRequiredService<DeleteProductFromCartStrategy>(),
                MessageTypes.ClearCart => _serviceProvider.GetRequiredService<DeleteAllProductsFromCartStrategy>(),
                _ => null
            };
        }
    }
}
