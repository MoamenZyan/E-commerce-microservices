using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductItemDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
