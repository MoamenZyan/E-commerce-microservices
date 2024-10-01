using MediatR;
using UserService.Application.DTOs;

namespace UserService.Application.Features.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<UserDto?>
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
    }
}
