using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;

namespace UserService.Application.Features.Commands.ClientSignup
{
    public class AdminSIgnupCommandHandler : IRequestHandler<AdminSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminSIgnupCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(AdminSignupCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = request.UserName;
            user.Email = request.Email;

            try
            {
                await _userManager.CreateAsync(user, request.Password);
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            catch (Exception)
            {
                throw;
            }

            return Unit.Value;
        }
    }
}
