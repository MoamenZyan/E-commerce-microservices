using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public OrderStatus Status { get; set; }
        public string ExternalId { get; set; } = null!;
        public decimal Total { get; set; }
        public DateTime IssuedAt { get; set; }

        public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
