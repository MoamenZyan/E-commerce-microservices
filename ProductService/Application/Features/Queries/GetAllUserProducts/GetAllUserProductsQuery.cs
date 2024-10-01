using MediatR;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllUserProducts
{
    public class GetAllUserProductsQuery : IRequest<List<Product>>
    {
        public required Guid UserId { get; set; }
    }
}
