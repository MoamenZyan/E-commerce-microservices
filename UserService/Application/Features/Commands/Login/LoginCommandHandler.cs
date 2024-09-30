using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Shared.Entities;
using UserService.Application.Common.Responses;
using UserService.Infrastructure.Utilities;

namespace UserService.Application.Features.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<object>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<object>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Result<object> response = new Result<object>();

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.Errors = new string[]
                {
                    "user not found"
                };
                response.IsSuccess = false;
                response.Data = null;

                return response;
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                response.Errors = new string[]
                {
                    "Email/Password Incorrect"
                };
                response.IsSuccess = false;
                response.Data = null;

                Log.Warning($"User {user.Id} has failed login attempt");
                return response;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var token = JWTService.GenerateJWTToken(user, userRoles);


            response.Errors = null;
            response.IsSuccess = true;
            response.Data = new {token = token};

            Log.Information($"User {user.Id} has logged in successfully");

            return response;
        }
    }
}
