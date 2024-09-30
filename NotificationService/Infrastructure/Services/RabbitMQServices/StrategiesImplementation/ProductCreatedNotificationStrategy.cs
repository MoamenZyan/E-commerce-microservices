
using MediatR;
using Newtonsoft.Json;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.EmailServices;


namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class ProductCreatedNotificationStrategy : IProductCreatedNotificationStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public ProductCreatedNotificationStrategy(IServiceProvider serviceProvider)
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
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var productCreatedEmailStrategy = scope.ServiceProvider.GetRequiredService<IProductCreatedEmailStrategy>();
                    emailSenderContext.SetStrategy(productCreatedEmailStrategy);
                    var notificationBody = $"Your product \"{messageObj.product.Name}\" has created!";

                    CreateNormalNotificationCommand createNormalNotificationCommand = new CreateNormalNotificationCommand()
                    {
                        UserId = messageObj.product.OwnerId,
                        Body = notificationBody
                    };

                    await mediator.Send(createNormalNotificationCommand);
                    await emailSenderContext.Send(Convert.ToString(messageObj.product.Name), Convert.ToString(messageObj.Email), notificationBody);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
