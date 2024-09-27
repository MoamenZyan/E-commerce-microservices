using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Infrastructure.Configurations.SendGridConfiguration;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Options;

namespace NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation
{
    public class ResetPasswordEmailStrategy : IResetPasswordEmailStrategy
    {
        private readonly SendGridConfiguration _sendGridConfiguration;
        public ResetPasswordEmailStrategy(IOptions<SendGridConfiguration> sendGridConfiguration)
        {
            _sendGridConfiguration = sendGridConfiguration.Value;
        }
        public async Task Send(string userName, string email, string body)
        {
            var client = new SendGridClient(_sendGridConfiguration.ApiKey);
            var from = new EmailAddress(_sendGridConfiguration.FromEmail, "E-Commerce-Microservices");
            var subject = "Reset Password Request";
            var to = new EmailAddress(email, userName);
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
