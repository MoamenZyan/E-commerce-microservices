using MediatR;
using NotificationService.Application.Common;
using NotificationService.Infrastructure.Data;

namespace NotificationService.Application.Features.Queries.GetAllUserNotifications
{
    public class GetAllUserNotificationQueryHandler : IRequestHandler<GetAllUserNotificationQuery, GetAllUserNotificationsResponse>
    {
        private readonly ApplicationDbContext _context;
        public GetAllUserNotificationQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<GetAllUserNotificationsResponse> Handle(GetAllUserNotificationQuery request, CancellationToken cancellationToken)
        {
            var response = new GetAllUserNotificationsResponse();
            try
            {
                var normalNotifications = _context.NormalNotifications.Where(x => x.UserId == request.UserId).ToList();
                var orderNotifications = _context.OrderNotifications.Where(x => x.UserId == request.UserId).ToList();

                response.OrderNotifications = orderNotifications;
                response.NormalNotifications = normalNotifications;
                response.IsSuccess = true;
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>
                {
                    ex.Message,
                };
                return Task.FromResult(response);
            }
        }
    }
}
