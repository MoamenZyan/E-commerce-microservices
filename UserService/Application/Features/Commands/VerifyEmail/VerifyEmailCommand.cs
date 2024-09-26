using MediatR;

namespace UserService.Application.Features.Commands.VerifyEmail
{
    public class VerifyEmailCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
    }
}
