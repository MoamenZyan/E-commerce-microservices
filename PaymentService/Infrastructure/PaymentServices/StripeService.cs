using PaymentService.Application.Features.Commands.CreatePaymentOrder;
using PaymentService.Application.Interfaces;

namespace PaymentService.Infrastructure.PaymentServices
{
    public class StripeService : IPayment
    {
        public Task<dynamic> Pay(CreatePaymentOrderCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
