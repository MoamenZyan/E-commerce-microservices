using MediatR;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProductsInfo
{
    public class GetProductsInfoQuery : IRequest<List<Product>>
    {
        public required List<Guid> ProductsIds { get; set; }
    }
}
