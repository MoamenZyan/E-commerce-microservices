
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services.ExternalHttpServices;
using Shared.Enums;

namespace OrderService.Infrastructure.Services.BackgroundServices
{
    public class PaypalOrderCheckService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public PaypalOrderCheckService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var changed = false;
                    var task = new List<Task>();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var externalHttpService = scope.ServiceProvider.GetRequiredService<ExternalHttpService>();
                    if (dbContext == null)
                        throw new ArgumentNullException(nameof(dbContext));

                    var orders = await dbContext.Orders.Where(x => x.Status != Shared.Enums.OrderStatus.COMPLETED).ToListAsync();
                    foreach (var order in orders)
                    {
                        task.Add(Task.Run(async () =>
                        {
                            dynamic? result = await externalHttpService.CheckOrder(order.ExternalId);
                            if (result != null)
                            {
                                var newStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), Convert.ToString(result.status));
                                if (order.Status != newStatus)
                                {
                                    changed = true;
                                    order.Status = newStatus;
                                }
                            }
                        }));
                    }

                    await Task.WhenAll(task);
                    if (changed)
                    {
                        dbContext.Orders.UpdateRange(orders);
                        await dbContext.SaveChangesAsync();
                    }
                }
                await Task.Delay(10_000, stoppingToken);
            }
            return;
        }
    }
}
