using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RabbitMQService _rabbitmqService;
        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager, RabbitMQService rabbitmqService)
        {
            _userManager = userManager;
            _rabbitmqService = rabbitmqService;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var obj = new
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = token,
                Type = "confirmEmail"
            };
            _rabbitmqService.SendNotification(obj);
            return true;
        }
    }
}
