namespace Shared.Entities
{
    public class PaypalOrder
    {
        public required string Id { get; set; }
        public required string Status { get; set; }
        public List<PaypalLink> Links { get; set; } = new List<PaypalLink>();
    }

    public class PaypalLink
    {
        public required string Href { get; set; }
        public required string Rel { get; set; }
        public required string Method { get; set; }
    }
}
