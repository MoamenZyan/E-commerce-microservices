using MediatR;
using Shared.DTOs;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
