using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Shared.Entities;
using Shared.Enums;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Commands.ResetPasswordRequest
{
    public class ResetPasswordRequestCommandHandler : IRequestHandler<ResetPasswordRequestCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ResetPasswordRequestCommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<Unit> Handle(ResetPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                Log.Error($"No user found with this id {request.UserId}");
                throw new Exception("user not exist");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            dynamic obj = new
            {
                UserId = request.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Token = token,
            };

            OutboxMessage message = new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                MessageType = MessageTypes.ResetPassword,
                Content = JsonConvert.SerializeObject(obj),
            };

            await _context.OutboxMessages.AddAsync(message);
            await _context.SaveChangesAsync();


            Log.Information($"User {user.Id} has requested to reset his password");
            return Unit.Value;
        }
    }
}
