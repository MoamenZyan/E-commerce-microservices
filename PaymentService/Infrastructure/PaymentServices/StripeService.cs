using Newtonsoft.Json;
using PaymentService.Application.Features.Commands.CreatePaymentOrder;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Responses;
using Stripe.Checkout;
using System.Text;

namespace PaymentService.Infrastructure.PaymentServices
{
    public class StripeService : IPayment
    {
        public async Task<CheckoutResponse> Pay(CreateCheckoutCommand command)
        {
            var orderId = Guid.NewGuid();

            var products = new List<SessionLineItemOptions>();
            foreach (var product in command.Products)
            {
                var item = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (int)(product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = product.Name,
                            Description = product.Description,
                        }
                    },
                    Quantity = product.Quantity,
                };

                products.Add(item);
            }

            var textBytes = Encoding.UTF8.GetBytes($"{command.Email}:{orderId}");
            var base64String = Convert.ToBase64String(textBytes);

            var sessionOptions = new SessionCreateOptions()
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = products,
                Mode = "payment",
                SuccessUrl = $"http://localhost:5126/api/order/success/{base64String}",
            };
 
            var service = new SessionService();
            Session session = await service.CreateAsync(sessionOptions);

            return new CheckoutResponse
            {
                OrderId = orderId,
                IsSuccess = true,
                CheckoutId = session.Id,
                RedirectionUrl = session.Url
            };
        }
    }
}
