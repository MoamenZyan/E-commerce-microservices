using MediatR;
using Microsoft.AspNetCore.Identity;
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
                throw new Exception("user not exist");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
                throw new Exception("reset password not successfull");

            return Unit.Value;
        }
    }
}
