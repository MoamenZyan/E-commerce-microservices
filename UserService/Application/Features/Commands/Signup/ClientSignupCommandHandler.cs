using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Data;

namespace UserService.Application.Features.Commands.Signup
{
    public class ClientSignupCommandHandler : IRequestHandler<ClientSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ClientSignupCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(ClientSignupCommand request, CancellationToken cancellationToken)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = request.UserName;
            user.Email = request.Email;

            try
            {
                await _userManager.CreateAsync(user, request.Password);
                await _userManager.AddToRoleAsync(user, "Client");
            }
            catch (Exception)
            {
                throw;
            }

            return Unit.Value;
        }
    }
}
