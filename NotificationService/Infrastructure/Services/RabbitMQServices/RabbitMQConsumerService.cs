using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation;
using NotificationService.Application.Interfaces.NotificationStrategies;

namespace NotificationService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        public RabbitMQConsumerService(IConnection connection, IModel channel,
                                        IServiceProvider services)
        {
            _connection = connection;
            _channel = channel;
            _services = services;

            _channel.QueueDeclare(
                queue: "notification",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += ProcessMessage;

            _channel.BasicConsume(
                queue: "notification",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        private async Task ProcessMessage(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var obj = JsonConvert.DeserializeObject<dynamic>(message);

            if (obj == null)
                return;

            try
            {
                using (IServiceScope scope  = _services.CreateScope())
                {
                    NotificationStrategyContext? strategyContext = scope.ServiceProvider.GetService<NotificationStrategyContext>();
                    if (strategyContext == null)
                        throw new Exception("NotificationStrategyContext not registered");
                    
                    INotificationStrategy strategy = strategyContext.GetStrategy(Convert.ToString(obj.Type));
                    if (strategy != null)
                        await strategy.Process(obj);
                }

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
