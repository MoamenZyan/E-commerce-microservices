using MediatR;
using Newtonsoft.Json;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.EmailServices;
using NotificationService.Infrastructure.Services.ExternalHttpServices;
using Shared.DTOs;
using Shared.Entities;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class ProductAddedToCartNotificationStrategy : IProductAddedToCartNotificationStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductAddedToCartNotificationStrategy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Process(dynamic messageObj)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailSenderContext = scope.ServiceProvider.GetRequiredService<EmailSenderContext>();
                    var httpClient = scope.ServiceProvider.GetRequiredService<ExternalHttpService>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var productAddedToCartEmailStrategy = scope.ServiceProvider.GetRequiredService<IProductAddedToCartEmailStrategy>();


                    emailSenderContext.SetStrategy(productAddedToCartEmailStrategy);

                    ProductDto? product = await httpClient.GetProduct(Convert.ToString(messageObj.ProductId));
                    if (product == null)
                        throw new Exception("Product not exists");

                    var notificationBody = $"Product \"{product.Name}\" has been added to your cart!";

                    CreateNormalNotificationCommand createNormalNotificationCommand = new CreateNormalNotificationCommand()
                    {
                        UserId = product.OwnerId,
                        Body = notificationBody
                    };

                    await mediator.Send(createNormalNotificationCommand);
                    await emailSenderContext.Send(Convert.ToString(product.Name), Convert.ToString(messageObj.Email), notificationBody);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
