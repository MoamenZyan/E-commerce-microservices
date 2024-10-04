using MediatR;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllUserProducts
{
    public class GetAllUserProductsQuery : IRequest<List<ProductDto>>
    {
        public required Guid UserId { get; set; }
    }
}
