using MediatR;
using PaymentService.Application.Responses;

namespace PaymentService.Application.Features.Queries.CheckOrder
{
    public class CheckOrderQuery : IRequest<OrderCheckResponse?>
    {
        public required string OrderId { get; set; }
    }
}
