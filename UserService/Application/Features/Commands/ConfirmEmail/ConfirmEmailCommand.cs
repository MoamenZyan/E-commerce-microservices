using MediatR;

namespace UserService.Application.Features.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
    }
}
