using RabbitMQ.Client;
using NotificationService.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
