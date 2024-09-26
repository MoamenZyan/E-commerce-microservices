using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.EmailServices;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class WelcomeNotificationStrategy : IWelcomeNotificationStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public WelcomeNotificationStrategy(IServiceProvider serviceProvider)
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
                    var welcomeEmailStrategy = scope.ServiceProvider.GetRequiredService<IWelcomeEmailStrategy>();
                    emailSenderContext.SetStrategy(welcomeEmailStrategy);
                    var notificationBody = $"Welcome onboard {messageObj.UserName}";

                    CreateNormalNotificationCommand createNormalNotificationCommand = new CreateNormalNotificationCommand()
                    {
                        UserId = messageObj.UserId,
                        Body = notificationBody
                    };

                    await mediator.Send(createNormalNotificationCommand);
                    await emailSenderContext.Send(Convert.ToString(messageObj.UserName), Convert.ToString(messageObj.Email), notificationBody);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
