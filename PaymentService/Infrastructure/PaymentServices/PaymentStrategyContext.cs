using PaymentService.Application.Interfaces;
using Shared.Enums;

namespace PaymentService.Infrastructure.PaymentServices
{
    public class PaymentStrategyContext
    {
        private readonly IServiceProvider _serviceProvider;
        public PaymentStrategyContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IPayment? GetStrategy(PaymentType type)
        {
            return type switch
            {
                PaymentType.Paypal => _serviceProvider.GetRequiredService<PaypalService>(),
                PaymentType.Stripe => _serviceProvider.GetRequiredService<StripeService>(),
                _ => null
            };
        }
    }
}
