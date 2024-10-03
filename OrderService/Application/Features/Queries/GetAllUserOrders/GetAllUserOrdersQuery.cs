using MediatR;
using Shared.Entities;

namespace OrderService.Application.Features.Queries.GetAllUserOrders
{
    public class GetAllUserOrdersQuery : IRequest<List<Order>>
    {
        public required Guid UserId { get; set; }
    }
}
