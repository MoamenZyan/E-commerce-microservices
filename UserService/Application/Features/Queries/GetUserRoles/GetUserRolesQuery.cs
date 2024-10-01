using MediatR;

namespace UserService.Application.Features.Queries.GetUserRoles
{
    public class GetUserRolesQuery : IRequest<List<string>?>
    {
        public required string UserId { get; set; }
    }
}
