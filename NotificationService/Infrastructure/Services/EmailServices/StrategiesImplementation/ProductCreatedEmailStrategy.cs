using Microsoft.Extensions.Options;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Infrastructure.Configurations.SendGridConfiguration;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation
{
    public class ProductCreatedEmailStrategy : IProductCreatedEmailStrategy
    {
        private readonly SendGridConfiguration _sendGridConfiguration;
        public ProductCreatedEmailStrategy(IOptions<SendGridConfiguration> sendGridConfiguration)
        {
            _sendGridConfiguration = sendGridConfiguration.Value;
        }
        public async Task Send(string userName, string email, string body)
        {
            var client = new SendGridClient(_sendGridConfiguration.ApiKey);
            var from = new EmailAddress(_sendGridConfiguration.FromEmail, "E-Commerce-Microservices");
            var subject = "Product Created!";
            var to = new EmailAddress(email, userName);
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
