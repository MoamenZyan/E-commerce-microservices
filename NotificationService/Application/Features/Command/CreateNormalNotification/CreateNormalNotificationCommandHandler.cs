using MediatR;
using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Application.Features.Command.CreateNormalNotification
{
    public class CreateNormalNotificationCommandHandler : IRequestHandler<CreateNormalNotificationCommand, NormalNotification>
    {
        private readonly ApplicationDbContext _context;
        public CreateNormalNotificationCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<NormalNotification> Handle(CreateNormalNotificationCommand request, CancellationToken cancellationToken)
        {
            NormalNotification normalNotification = new NormalNotification();
            normalNotification.UserId = request.UserId;
            normalNotification.Id = Guid.NewGuid();
            normalNotification.CreatedAt = DateTime.UtcNow;
            normalNotification.Body = request.Body;

            await _context.NormalNotifications.AddAsync(normalNotification);
            await _context.SaveChangesAsync();
            return normalNotification;
        }
    }
}
