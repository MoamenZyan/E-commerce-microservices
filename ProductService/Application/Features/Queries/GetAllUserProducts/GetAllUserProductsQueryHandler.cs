using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllUserProducts
{
    public class GetAllUserProductsQueryHandler : IRequestHandler<GetAllUserProductsQuery, List<Product>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllUserProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> Handle(GetAllUserProductsQuery request, CancellationToken cancellationToken)
        {

            var products = await _context.Products.Where(x => x.OwnerId == request.UserId).ToListAsync();
            return products;
        }
    }
}
