using MediatR;
using Shared.Enums;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<string?>
    {
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
        public required string Email { get; set; }
        public required PaymentType PaymentType { get; set; }
    }
}
