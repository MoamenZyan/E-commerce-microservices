
using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Services.OutboxServices
{
    public class OutboxService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
