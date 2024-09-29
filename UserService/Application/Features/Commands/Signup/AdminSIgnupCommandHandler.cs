using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.Enums;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ClientSignup
{
    public class AdminSIgnupCommandHandler : IRequestHandler<AdminSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public AdminSIgnupCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

                dynamic obj = new
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                };

                OutboxMessage message = new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    MessageType = MessageTypes.Welcome,
                    Content = JsonConvert.SerializeObject(obj),
                };

                await _context.OutboxMessages.AddAsync(message);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }

            return Unit.Value;
        }
    }
}
