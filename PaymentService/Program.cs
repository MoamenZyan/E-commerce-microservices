using PaymentService.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
