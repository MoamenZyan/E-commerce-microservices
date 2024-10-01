using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Application.Features.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<string>?>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<string>?> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
    }
}
