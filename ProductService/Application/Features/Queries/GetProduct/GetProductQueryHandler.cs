using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product?>
    {
        private readonly ApplicationDbContext _context;
        public GetProductQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
                return product;
            }
            return null!;
        }
    }
}
