namespace NotificationService.Domain.Entities
{
    public class OrderNotification
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Body { get; set; } = null!;
    }
}
