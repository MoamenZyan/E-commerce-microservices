using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.Data;
using Shared.SigningKeys;
using System.Security.Cryptography;
using MediatR;
using ProductService.Infrastructure.Configurations.EntitiesConfigurations;
using RabbitMQ.Client;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using ProductService.Infrastructure.Services.RabbitMQServices;
using ProductService.Infrastructure.Services.OutboxServices;

namespace ProductService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                RSA rsa = RSA.Create();
                rsa.ImportFromPem(SigningKeys.GetPublicKey());
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = "Backend",
                    ValidAudience = "Frontend",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };
            });

            services.AddMediatR(typeof(ProductConfiguration));

            // RabbitMQ
            services.AddSingleton<IConnectionFactory>(sp =>
            {
                return new ConnectionFactory
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest",
                    DispatchConsumersAsync = true,
                };
            });

            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IConnectionFactory>();
                return factory.CreateConnection();
            });

            services.AddSingleton<IModel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });


            services.AddHostedService<OutboxService>();
            services.AddScoped<RabbitMQService>();

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
