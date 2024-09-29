
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Services
{
    public class OutboxService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public OutboxService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = new List<Task>();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    var rabbitMqService = scope.ServiceProvider.GetService<RabbitMQService>();

                    if (rabbitMqService == null)
                        throw new InvalidOperationException("RabbitMQ Service is not registered");

                    if (context == null)
                        throw new InvalidOperationException("DB Context is not registered");


                    var outBoxMessages = await context.OutboxMessages.Where(x => x.Processed == false).ToListAsync(stoppingToken);

                    foreach (var message in outBoxMessages)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            try
                            {
                                dynamic content = JsonConvert.DeserializeObject<dynamic>(message.Content)!;
                                var obj = new
                                {
                                    content,
                                    Type = message.MessageType,
                                };

                                var result = await rabbitMqService.SendNotification(obj);

                                if (result == true)
                                    message.Processed = true;
                                else
                                    Console.WriteLine($"Failed acknowledgement for message: {message.Id}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }));
                    }

                    await Task.WhenAll(tasks);
                    context.OutboxMessages.UpdateRange(outBoxMessages);
                    await context.SaveChangesAsync();
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
