namespace PaymentService.Application.Responses
{
    public class CheckoutResponse
    {
        public Guid OrderId { get; set; }
        public bool IsSuccess { get; set; }
        public string? CheckoutId { get; set; }
        public string? RedirectionUrl { get; set; }
    }
}
