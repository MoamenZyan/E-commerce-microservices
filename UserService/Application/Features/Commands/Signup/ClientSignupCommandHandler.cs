using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.Signup
{
    public class ClientSignupCommandHandler : IRequestHandler<ClientSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RabbitMQService _rabbitmqService;

        public ClientSignupCommandHandler(UserManager<ApplicationUser> userManager, RabbitMQService rabbitmqService)
        {
            _userManager = userManager;
            _rabbitmqService = rabbitmqService;
            _rabbitmqService = rabbitmqService;
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

                var obj = new
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Type = "welcome"
                };
                _rabbitmqService.SendNotification(obj);
            }
            catch (Exception)
            {
                throw;
            }

            return Unit.Value;
        }
    }
}
