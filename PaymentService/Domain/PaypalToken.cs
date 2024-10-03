namespace PaymentService.Domain
{
    public class PaypalToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpireDate { get; set; }
    }
}
