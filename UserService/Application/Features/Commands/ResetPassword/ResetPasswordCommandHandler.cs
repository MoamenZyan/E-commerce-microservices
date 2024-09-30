using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Entities;

namespace UserService.Application.Features.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                Log.Error($"No user found with this id {request.UserId}");
                throw new Exception("user not exist");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                Log.Error($"Reset password has failed for user {user.Id}");
                throw new Exception("reset password not successfull");
            }

            Log.Information($"User {user.Id} has reset his password successfully");
            return Unit.Value;
        }
    }
}
