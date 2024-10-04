using MediatR;

namespace OrderService.Application.Features.Commands.ConfirmOrder
{
    public class ConfirmOrderCommand : IRequest<bool>
    {
        public required string Email { get; set; }
        public required Guid OrderId { get; set; }
    }
}
