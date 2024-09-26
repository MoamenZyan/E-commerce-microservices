using MediatR;
using NotificationService.Infrastructure.Services.EmailServices;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class ConfirmEmailNotificationStrategy : IConfirmEmailNotificationStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public ConfirmEmailNotificationStrategy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Process(dynamic messageObj)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var emailSenderContext = scope.ServiceProvider.GetRequiredService<EmailSenderContext>();

                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var confirmEmailStrategy = scope.ServiceProvider.GetRequiredService<IConfirmEmailStrategy>();
                emailSenderContext.SetStrategy(confirmEmailStrategy);
                var url = $"http://localhost:8080/api/auth/verifyEmail?userId={messageObj.UserId}&token={Uri.EscapeDataString(Convert.ToString(messageObj.Token))}";
                var notificationBody = $"{messageObj.UserName}, here is your confirmation link: {url}";

                await emailSenderContext.Send(Convert.ToString(messageObj.UserName), Convert.ToString(messageObj.Email), notificationBody);
            }
        }
    }
}
