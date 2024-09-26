using MediatR;
using NotificationService.Application.Common;

namespace NotificationService.Application.Features.Queries.GetAllUserNotifications
{
    public class GetAllUserNotificationQuery : IRequest<GetAllUserNotificationsResponse>
    {
        public Guid UserId { get; set; }
    }
}
