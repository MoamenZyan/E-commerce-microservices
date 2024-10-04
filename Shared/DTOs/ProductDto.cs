using Shared.Entities;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CategoryTypes Category { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Review>? Reviews { get; set; } = null!;
        public ProductDto(Product product, List<Review> reviews)
        {
            Id = product.Id;
            OwnerId = product.OwnerId;
            Name = product.Name;
            Description = product.Description;
            Category = product.Category;
            Price = product.Price;
            Discount = product.Discount;
            CreatedAt = product.CreatedAt;
            Reviews = reviews;
        }
    }
}
