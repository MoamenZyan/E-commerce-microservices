namespace OrderService.Domain
{
    public class Checkout
    {
        public Guid OrderId { get; set; }
        public string? CheckoutId { get; set; }
        public required bool IsSuccess { get; set; }
        public string? RedirectionUrl { get; set; }
    }
}
