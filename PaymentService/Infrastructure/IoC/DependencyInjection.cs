using Microsoft.AspNetCore.Authentication;
using PaymentService.Infrastructure.AuthenticationSchemes;
using PaymentService.Infrastructure.PaymentServices;
using MediatR;
using PaymentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PaymentService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers();
            services.AddScoped<HttpClient>();

            // Authentication
            services.AddAuthentication("APIKey")
                    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("APIKey", null);

            services.AddMediatR(typeof(PaymentStrategyContext));
            services.AddScoped<HttpClient>();
            services.AddScoped<PaymentStrategyContext>();

            services.AddScoped<PaypalService>();
            services.AddScoped<StripeService>();
            return services;
        }
    }
}
