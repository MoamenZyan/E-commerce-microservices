using MediatR;
using UserService.Application.Common.Responses;

namespace UserService.Application.Features.Commands.Login
{
    public class LoginCommand : IRequest<Result<object>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
