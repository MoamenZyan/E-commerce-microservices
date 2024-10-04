using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Services.ExternalHttpServices;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProductsInfo
{
    public class GetProductsInfoQueryHandler : IRequestHandler<GetProductsInfoQuery, List<ProductDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ExternalHttpService _externalHttpService;
        public GetProductsInfoQueryHandler(ApplicationDbContext context, ExternalHttpService externalHttpService)
        {
            _context = context;
            _externalHttpService = externalHttpService;
        }
        public async Task<List<ProductDto>> Handle(GetProductsInfoQuery request, CancellationToken cancellationToken)
        {
            List<Product> products = new List<Product>();
            products = await _context.Products.Where(x => request.ProductsIds.Contains(x.Id)).ToListAsync();
            Dictionary<Guid, List<Review>> reviews = await _externalHttpService.GetProductsReviews(request.ProductsIds);

            var productsDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                ProductDto productDto = new ProductDto(product, reviews.FirstOrDefault(x => x.Key == product.Id).Value);
                productsDtos.Add(productDto);
            }

            return productsDtos;
        }

    }
}
