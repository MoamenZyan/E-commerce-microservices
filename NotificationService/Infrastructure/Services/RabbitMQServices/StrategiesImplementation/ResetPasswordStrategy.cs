using MediatR;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.EmailServices;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class ResetPasswordStrategy : IResetPasswordStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public ResetPasswordStrategy(IServiceProvider serviceProvider)
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
                    var resetPasswordEmailStrategy = scope.ServiceProvider.GetRequiredService<IResetPasswordEmailStrategy>();
                    emailSenderContext.SetStrategy(resetPasswordEmailStrategy);

                    var url = $"http://localhost:8080/api/auth/resetPassword?userId={messageObj.UserId}&token={Uri.EscapeDataString(Convert.ToString(messageObj.Token))}";

                    var notificationBody = $"{messageObj.UserName}, Reset Password Link: {url}";
                    
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
