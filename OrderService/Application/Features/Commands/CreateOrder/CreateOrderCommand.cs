using MediatR;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<string?>
    {
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
    }
}
