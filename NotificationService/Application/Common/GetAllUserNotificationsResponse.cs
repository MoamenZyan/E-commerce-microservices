using NotificationService.Domain.Entities;

namespace NotificationService.Application.Common
{
    public class GetAllUserNotificationsResponse
    {
        public List<NormalNotification>? NormalNotifications { get; set; } = new List<NormalNotification>();
        public List<OrderNotification>? OrderNotifications { get; set; } = new List<OrderNotification>();
        public bool IsSuccess { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();
    }
}
