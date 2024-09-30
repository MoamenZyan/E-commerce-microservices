using Serilog;
using Microsoft.AspNetCore.Identity;
using UserService.Infrastructure.IoC;
using UserService.Infrastructure.RoleSeedData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Adding DI Container
builder.Services.AddServices(builder.Configuration);


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeedData.SeedData(roleManager);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

Log.Information("User Service Started");
app.Run();
