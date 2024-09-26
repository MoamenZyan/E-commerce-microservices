namespace NotificationService.Domain.Entities
{
    public class NormalNotification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Body { get; set; } = null!;
    }
}
