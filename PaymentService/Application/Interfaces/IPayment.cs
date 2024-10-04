using PaymentService.Application.Features.Commands.CreatePaymentOrder;
using PaymentService.Application.Responses;

namespace PaymentService.Application.Interfaces
{
    public interface IPayment
    {
        Task<CheckoutResponse> Pay(CreateCheckoutCommand command);
    }
}
