using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.ToListAsync();
        }
    }
}
