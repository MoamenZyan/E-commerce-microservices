using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;

namespace CartService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQProducerService

    {
        private readonly IModel _channel;
        public RabbitMQProducerService(IModel channel)
        {
            _channel = channel;
            _channel.QueueDeclare("cart", true, false, false, null);
        }

        public async Task<bool> SendMessage(object obj)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(serializedObj);

            _channel.ConfirmSelect();
            _channel.BasicPublish(
                exchange: "amq.direct",
                routingKey: "cart",
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
