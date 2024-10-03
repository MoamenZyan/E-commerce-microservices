using MediatR;
using PaymentService.Application.Responses;
using PaymentService.Infrastructure.PaymentServices;

namespace PaymentService.Application.Features.Queries.CheckOrder
{
    public class CheckOrderQueryHandler : IRequestHandler<CheckOrderQuery, OrderCheckResponse?>
    {
        private readonly PaypalService _paypalService;
        public CheckOrderQueryHandler(PaypalService paypalService)
        {
            _paypalService = paypalService;
        }
        public Task<OrderCheckResponse?> Handle(CheckOrderQuery request, CancellationToken cancellationToken)
        {
            return _paypalService.CheckOrder(request.OrderId);
        }
    }
}
