using MediatR;
using Shared.Entities;

namespace ProductService.Application.Features.Queries.GetProduct
{
    public class GetProductQuery : IRequest<Product?>
    {
        public required Guid ProductId { get; set; }
    }
}
