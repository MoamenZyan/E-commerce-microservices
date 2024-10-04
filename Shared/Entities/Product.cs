using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CategoryTypes Category { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Review>? Reviews { get; set; } = new List<Review>();
    }
}
