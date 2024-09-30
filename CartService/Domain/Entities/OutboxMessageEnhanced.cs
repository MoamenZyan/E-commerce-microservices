using Shared.Entities;

namespace CartService.Domain.Entities
{
    public class OutboxMessageCart : OutboxMessage
    {
        public bool IsNotification { get; set; }
    }
}
