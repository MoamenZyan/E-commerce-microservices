using PaymentService.Application.Features.Commands.CreatePaymentOrder;

namespace PaymentService.Application.Interfaces
{
    public interface IPayment
    {
        Task<dynamic> Pay(CreatePaymentOrderCommand command);
    }
}
