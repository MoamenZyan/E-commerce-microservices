using Shared.Entities;

namespace CartService.Domain.Entities
{
    public class ProductCart
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
