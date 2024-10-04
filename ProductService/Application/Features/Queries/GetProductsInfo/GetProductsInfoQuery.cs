using MediatR;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProductsInfo
{
    public class GetProductsInfoQuery : IRequest<List<ProductDto>>
    {
        public required List<Guid> ProductsIds { get; set; }
    }
}
