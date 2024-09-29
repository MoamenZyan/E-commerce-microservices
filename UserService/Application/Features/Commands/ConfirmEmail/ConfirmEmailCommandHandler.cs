using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Shared.Entities;
using Shared.Enums;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return false;
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            dynamic obj = new
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Token = token,
            };

            OutboxMessage message = new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                MessageType = MessageTypes.ConfirmEmail,
                Content = JsonConvert.SerializeObject(obj),
            };

            await _context.OutboxMessages.AddAsync(message);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
