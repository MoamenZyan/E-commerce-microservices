using NotificationService.Application.EmailStrategies;

namespace NotificationService.Infrastructure.Services.EmailServices
{
    public class EmailSenderContext
    {
        private IEmailStrategy _emailStrategy = null!;

        public void SetStrategy(IEmailStrategy emailStrategy)
        {
            _emailStrategy = emailStrategy;
        }

        public async Task Send(string userName, string email, string body)
        {
            await _emailStrategy.Send(userName, email, body);
        }
    }
}
