using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.Enums;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.Signup
{
    public class ClientSignupCommandHandler : IRequestHandler<ClientSignupCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ClientSignupCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
