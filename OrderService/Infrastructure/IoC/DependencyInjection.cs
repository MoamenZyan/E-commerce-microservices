using OrderService.Infrastructure.Services.ExternalHttpServices;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using OrderService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using RabbitMQ.Client;
using OrderService.Infrastructure.Services.RabbitMQServices;
using OrderService.Infrastructure.Services.OutboxServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Shared.SigningKeys;
using System.Security.Cryptography;
namespace OrderService.Infrastructure.IoC
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

            services.AddScoped<HttpClient>();
            services.AddScoped<ExternalHttpService>();


            services.AddMediatR(typeof(Program));

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                RSA rsa = RSA.Create();
                rsa.ImportFromPem(SigningKeys.GetPublicKey());
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    SaveSigninToken = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidAudience = "Frontend",
                    ValidIssuer = "Backend",
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };
            });


            // RabbitMQ
            services.AddSingleton<RabbitMQ.Client.IConnectionFactory>(sp =>
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
                var factory = sp.GetRequiredService<RabbitMQ.Client.IConnectionFactory>();
                return factory.CreateConnection();
            });

            services.AddSingleton<IModel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });

            services.AddScoped<RabbitMQProducerService>();

            

            // Background Services
            services.AddHostedService<RabbitMQConsumerService>();
            services.AddHostedService<OutboxService>();

            // Http Services
            services.AddScoped<HttpClient>();
            services.AddScoped<ExternalHttpService>();

            return services;
        }
    }
}
