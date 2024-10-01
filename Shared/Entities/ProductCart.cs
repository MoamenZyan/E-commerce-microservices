

namespace Shared.Entities
{
    public class ProductCart
    {
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; } = null!;
    }
}
