using Microsoft.Extensions.Options;
using NotificationService.Infrastructure.Configurations.SendGridConfiguration;
using SendGrid.Helpers.Mail;
using SendGrid;
using NotificationService.Application.Interfaces.EmailStrategies;

namespace NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation
{
    public class OrderConfirmedEmailStrategy : IOrderConfirmedEmailStrategy
    {
        private readonly SendGridConfiguration _sendGridConfiguration;
        public OrderConfirmedEmailStrategy(IOptions<SendGridConfiguration> sendGridConfiguration)
        {
            _sendGridConfiguration = sendGridConfiguration.Value;
        }
        public async Task Send(string userName, string email, string body)
        {
            var client = new SendGridClient(_sendGridConfiguration.ApiKey);
            var from = new EmailAddress(_sendGridConfiguration.FromEmail, "E-Commerce-Microservices");
            var subject = "Order Confirmed!";
            var to = new EmailAddress(email, userName);
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
