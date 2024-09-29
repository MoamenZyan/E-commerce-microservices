using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public MessageTypes MessageType { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool Processed { get; set; }
    }
}
