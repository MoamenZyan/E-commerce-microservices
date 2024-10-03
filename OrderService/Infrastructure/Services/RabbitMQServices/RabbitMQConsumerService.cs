
namespace OrderService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQConsumerService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
