using MediatR;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
    }
}
