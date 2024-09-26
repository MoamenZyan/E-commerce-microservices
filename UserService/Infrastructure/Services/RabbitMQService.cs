using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Entities;
using System.Text;

namespace UserService.Infrastructure.Services
{
    public class RabbitMQService
    {
        private readonly IModel _channel;
        public RabbitMQService(IModel channel)
        {
            _channel = channel;
        }

        public void SendNotification(ApplicationUser user, string type)
        {
            var obj = new
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Type = type
            };
            var serializedObj = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(serializedObj);

            _channel.BasicPublish(
                exchange: "amq.direct",
                routingKey: "notification",
                basicProperties: null,
                body: body
            );
        }
    }
}
