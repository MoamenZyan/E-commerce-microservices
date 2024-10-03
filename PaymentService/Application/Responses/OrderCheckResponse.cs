namespace PaymentService.Application.Responses
{
    public class OrderCheckResponse
    {
        public required string Id { get; set; }
        public required string Intent { get; set; }
        public required string Status {  get; set; }
    }
}
