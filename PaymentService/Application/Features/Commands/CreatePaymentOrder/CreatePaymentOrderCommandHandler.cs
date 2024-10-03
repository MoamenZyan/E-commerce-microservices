using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Responses;
using PaymentService.Infrastructure.PaymentServices;
using Shared.Enums;

namespace PaymentService.Application.Features.Commands.CreatePaymentOrder
{
    public class CreatePaymentOrderCommandHandler : IRequestHandler<CreatePaymentOrderCommand, OrderPaymentCreationResponse>
    {
        private readonly PaymentStrategyContext _paymentStrategyContext;
        public CreatePaymentOrderCommandHandler(PaymentStrategyContext paymentStrategyContext)
        {
            _paymentStrategyContext = paymentStrategyContext;
        }
        public async Task<OrderPaymentCreationResponse> Handle(CreatePaymentOrderCommand request, CancellationToken cancellationToken)
        {
            IPayment paymentStrategy = _paymentStrategyContext.GetStrategy((PaymentType)Enum.Parse(typeof(PaymentType), request.PaymentType!))!;
            if (paymentStrategy == null)
                return new OrderPaymentCreationResponse()
                {
                    IsSuccess = false,
                };

            var obj = await paymentStrategy.Pay(request);
            return new OrderPaymentCreationResponse()
            {
                IsSuccess = true,
                Content = obj,
            };
        }
    }
}
