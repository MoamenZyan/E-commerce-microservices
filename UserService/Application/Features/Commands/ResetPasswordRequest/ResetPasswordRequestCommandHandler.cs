using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ResetPasswordRequest
{
    public class ResetPasswordRequestCommandHandler : IRequestHandler<ResetPasswordRequestCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RabbitMQService _rabbitmqService;

        public ResetPasswordRequestCommandHandler(UserManager<ApplicationUser> userManager, RabbitMQService rabbitmqService)
        {
            _userManager = userManager;
            _rabbitmqService = rabbitmqService;
        }
        public async Task<Unit> Handle(ResetPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("user doesn't exist");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var obj = new
            {
                UserId = request.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
                Type = "passwordReset"
            };

            _rabbitmqService.SendNotification(obj);
            return Unit.Value;
        }
    }
}
