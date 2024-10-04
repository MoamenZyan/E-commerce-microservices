using MediatR;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProduct
{
    public class GetProductQuery : IRequest<ProductDto?>
    {
        public required Guid ProductId { get; set; }
    }
}
