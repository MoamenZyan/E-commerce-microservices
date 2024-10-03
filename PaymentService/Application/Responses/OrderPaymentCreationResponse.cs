namespace PaymentService.Application.Responses
{
    public class OrderPaymentCreationResponse
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public dynamic? Content { get; set; }
    }
}
