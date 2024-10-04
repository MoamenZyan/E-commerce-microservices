
using CartService.Infrastructure.Data;
using CartService.Infrastructure.Services.RabbitMQServices.MessageProcessingStrategies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Entities;
using Shared.Enums;
using System.Text;

namespace CartService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        public RabbitMQConsumerService(IModel channel, IServiceProvider services)
        {
            _channel = channel;
            _services = services;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += ProcessMessage;

                _channel.BasicConsume("product", false, consumer);
                _channel.BasicConsume("order", false, consumer);
            }
            return Task.CompletedTask;
        }

        private async Task ProcessMessage(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(message)!;
            if (obj != null)
            {
                using (var scope = _services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<MessageProcessingContext>();
                    try
                    {
                        IMessageProcessingStrategy? messageProcessingStrategy = context.GetStrategy((MessageTypes)Enum.Parse(typeof(MessageTypes), Convert.ToString(obj.Type)));
                        if (messageProcessingStrategy != null)
                        {
                            await messageProcessingStrategy.Process(obj);
                            _channel.BasicAck(ea.DeliveryTag, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

    }
}
