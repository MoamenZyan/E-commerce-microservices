using MediatR;
using UserService.Application.Common.Responses;

namespace UserService.Application.Features.Commands.Signup
{
    public class ClientSignupCommand : IRequest
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
