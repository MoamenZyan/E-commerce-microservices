using ProductService.Infrastructure.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("Product Service Started");
app.Run();
