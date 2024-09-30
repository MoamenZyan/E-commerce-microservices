using MediatR;

namespace CartService.Application.Features.Commands.AddProductToCart
{
    public class AddProductToCartCommand : IRequest<bool>
    {
        public required Guid ProductId { get; set; }
        public required Guid UserId { get; set; }
        public int Quantity { get; set; }
        public required string Email { get; set; }
    }
}
