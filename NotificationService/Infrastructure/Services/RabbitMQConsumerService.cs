using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using MediatR;
using Newtonsoft.Json;
using NotificationService.Infrastructure.Services.EmailServices;
using NotificationService.Application.EmailStrategies;
using NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation;
using NotificationService.Application.Features.Command.CreateNormalNotification;

namespace NotificationService.Infrastructure.Services
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

            string notificationBody = null!;
            using (var scope = _services.CreateScope())
            {
                try
                {
                    var senderContext = scope.ServiceProvider.GetRequiredService<EmailSenderContext>();
                    if (obj.Type == "welcome")
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var welcomeEmailStrategy = scope.ServiceProvider.GetRequiredService<WelcomeEmailStrategy>();
                        senderContext.SetStrategy(welcomeEmailStrategy);
                        notificationBody = $"Welcome onboard {obj.UserName}";

                        CreateNormalNotificationCommand createNormalNotificationCommand = new CreateNormalNotificationCommand()
                        {
                            UserId = obj.UserId,
                            Body = notificationBody
                        };
                        await mediator.Send(createNormalNotificationCommand);
                    }
                    await senderContext.Send(Convert.ToString(obj.UserName), Convert.ToString(obj.Email), notificationBody);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
