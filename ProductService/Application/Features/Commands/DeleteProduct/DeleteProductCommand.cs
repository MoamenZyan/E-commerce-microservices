using MediatR;

namespace ProductService.Application.Features.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required Guid ProductId { get; set; }
        public required string Token { get; set; }
    }
}
