using RabbitMQ.Client;
using Serilog;
using NotificationService.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Notification User Started");
app.Run();
