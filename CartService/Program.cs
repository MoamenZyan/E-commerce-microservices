using CartService.Infrastructure.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServices(builder.Configuration);

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Cart Service Started");
app.Run();
