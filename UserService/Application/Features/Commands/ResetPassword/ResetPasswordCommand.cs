using MediatR;

namespace UserService.Application.Features.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
    }
}
