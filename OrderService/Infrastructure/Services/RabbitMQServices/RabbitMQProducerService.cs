using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Entities;
using System.Text;
namespace OrderService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQProducerService
    {
        private readonly IModel _channel;
        public RabbitMQProducerService(IModel channel)
        {
            _channel = channel;
            _channel.ConfirmSelect();
            _channel.QueueDeclare("order", true, false, false, null);
        }

        public async Task<bool> SendMessage(object obj, string routingKey)
        {
            var serializedObj = JsonConvert.SerializeObject(obj);
            var body = Encoding.UTF8.GetBytes(serializedObj);

            _channel.ConfirmSelect();
            _channel.BasicPublish(
                exchange: "amq.direct",
                routingKey: routingKey,
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
