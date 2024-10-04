using Microsoft.AspNetCore.Authentication;
using PaymentService.Infrastructure.AuthenticationSchemes;
using PaymentService.Infrastructure.PaymentServices;
using MediatR;
using PaymentService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Serilog.Sinks.Elasticsearch;
using Serilog;

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

            StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            services.AddScoped<PaypalService>();
            services.AddScoped<StripeService>();


            // Serilog configuration
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = "logs-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .CreateLogger();

            return services;
        }
    }
}
