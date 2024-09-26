using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Entities;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ClientSignup
{
    public class AdminSIgnupCommandHandler : IRequestHandler<AdminSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RabbitMQService _rabbitmqService;
        public AdminSIgnupCommandHandler(UserManager<ApplicationUser> userManager, RabbitMQService rabbitmqService)
        {
            _userManager = userManager;
            _rabbitmqService = rabbitmqService;
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
