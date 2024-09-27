using MediatR;

namespace UserService.Application.Features.Commands.ResetPasswordRequest
{
    public class ResetPasswordRequestCommand : IRequest
    {
        public required string UserId { get; set; }
    }
}
