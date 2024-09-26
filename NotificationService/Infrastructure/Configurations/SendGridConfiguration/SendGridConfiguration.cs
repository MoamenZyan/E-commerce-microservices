namespace NotificationService.Infrastructure.Configurations.SendGridConfiguration
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; } = null!;
        public string FromEmail { get; set; } = null!;
    }
}
