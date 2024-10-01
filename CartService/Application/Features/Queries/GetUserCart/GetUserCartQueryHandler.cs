using CartService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace CartService.Application.Features.Queries.GetUserCart
{
    public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, Cart?>
    {
        private readonly ApplicationDbContext _context;
        public GetUserCartQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Cart?> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
        {
            var cart  = await _context.Carts.Include(x => x.Products).FirstOrDefaultAsync(x => x.UserId == request.UserId);
            return cart;
        }
    }
}
