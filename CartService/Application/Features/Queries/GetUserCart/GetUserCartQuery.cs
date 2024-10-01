using MediatR;
using Shared.Entities;

namespace CartService.Application.Features.Queries.GetUserCart
{
    public class GetUserCartQuery : IRequest<Cart?>
    {
        public required Guid UserId { get; set; }
    }
}
