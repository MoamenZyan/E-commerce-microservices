using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Services.ExternalHttpServices;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Shared.DTOs.ProductDto?>
    {
        private readonly ApplicationDbContext _context;
        private readonly ExternalHttpService _externalHttpService;
        public GetProductQueryHandler(ApplicationDbContext context, ExternalHttpService externalHttpService)
        {
            _context = context;
            _externalHttpService = externalHttpService;
        }
        public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);

                if (product == null)
                    return null;

                var reviews = await _externalHttpService.GetProductReviews(product.Id);

                ProductDto productDto = new ProductDto(product, reviews);
                return productDto;
            }
            return null!;
        }
    }
}
