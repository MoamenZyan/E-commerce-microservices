using MediatR;
using NotificationService.Domain.Entities;

namespace NotificationService.Application.Features.Command.CreateNormalNotification
{
    public class CreateNormalNotificationCommand : IRequest<NormalNotification>
    {
        public Guid UserId { get; set; }
        public string Body { get; set; } = null!;
    }
}
