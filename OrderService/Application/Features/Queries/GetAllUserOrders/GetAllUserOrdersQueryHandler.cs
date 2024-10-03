using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;
using Shared.Entities;

namespace OrderService.Application.Features.Queries.GetAllUserOrders
{
    public class GetAllUserOrdersQueryHandler : IRequestHandler<GetAllUserOrdersQuery, List<Order>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllUserOrdersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> Handle(GetAllUserOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.Items)
                                                .Where(x => x.UserId == request.UserId)
                                                .ToListAsync();
            return orders;
        }
    }
}
