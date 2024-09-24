using MediatR;

namespace UserService.Application.Features.Commands.ClientSignup
{
    public class AdminSignupCommand : IRequest
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
