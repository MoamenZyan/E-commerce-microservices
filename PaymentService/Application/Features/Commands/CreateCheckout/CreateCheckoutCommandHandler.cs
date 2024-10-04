using MediatR;
using Newtonsoft.Json;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Responses;
using PaymentService.Infrastructure.PaymentServices;
using Shared.Enums;

namespace PaymentService.Application.Features.Commands.CreatePaymentOrder
{
    public class CreateCheckoutCommandHandler : IRequestHandler<CreateCheckoutCommand, CheckoutResponse>
    {
        private readonly PaymentStrategyContext _paymentStrategyContext;
        public CreateCheckoutCommandHandler(PaymentStrategyContext paymentStrategyContext)
        {
            _paymentStrategyContext = paymentStrategyContext;
        }
        public async Task<CheckoutResponse> Handle(CreateCheckoutCommand request, CancellationToken cancellationToken)
        {
            IPayment paymentStrategy = _paymentStrategyContext.GetStrategy((PaymentType)Enum.Parse(typeof(PaymentType), request.PaymentType!))!;
            if (paymentStrategy == null)
                return new CheckoutResponse()
                {
                    IsSuccess = false,
                };

            return await paymentStrategy.Pay(request);
        }
    }
}
