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

        public async Task<bool> SendNotification(object obj)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(serializedObj);

            _channel.ConfirmSelect();
            _channel.BasicPublish(
                exchange: "amq.direct",
                routingKey: "notification",
                basicProperties: null,
                body: body
            );

            try
            {
                var result = await Task.Run(() => _channel.WaitForConfirms(TimeSpan.FromSeconds(30)));
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
