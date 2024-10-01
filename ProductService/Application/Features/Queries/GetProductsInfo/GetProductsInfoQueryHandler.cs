using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProductsInfo
{
    public class GetProductsInfoQueryHandler : IRequestHandler<GetProductsInfoQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;
        public GetProductsInfoQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetProductsInfoQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = new List<Product>();
            products = await _context.Products.Where(x => request.ProductsIds.Contains(x.Id)).ToListAsync();
            return products;
        }
    }
}
