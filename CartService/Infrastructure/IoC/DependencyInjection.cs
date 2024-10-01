using CartService.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using Shared.SigningKeys;
using System.Security.Cryptography;
using CartService.Infrastructure.Services.OutboxServices;
using CartService.Infrastructure.Services.RabbitMQServices;
using System.Net.Http;

namespace CartService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Authentication
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

            services.AddMediatR(typeof(Program));


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
            services.AddScoped<RabbitMQProducerService>();
            services.AddHostedService<RabbitMQConsumerService>();


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
