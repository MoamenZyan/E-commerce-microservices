using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Services.ExternalHttpServices;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ExternalHttpService _externalHttpService;
        public GetAllProductsQueryHandler(ApplicationDbContext context, ExternalHttpService externalHttpService)
        {
            _context = context;
            _externalHttpService = externalHttpService;
        }
        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync();

            Dictionary<Guid, List<Review>> reviews = await _externalHttpService.GetProductsReviews(products.Select(x => x.Id).ToList());

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
