using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Entities;

namespace UserService.Application.Features.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public VerifyEmailCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Description);
            }
            if (result.Succeeded)
            {
                Log.Information($"User {user.Id} has confirmed his email");
                return true;
            }


            
            return false;
        }
    }
}
