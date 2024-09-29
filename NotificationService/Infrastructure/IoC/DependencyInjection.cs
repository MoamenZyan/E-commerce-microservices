using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Data;
using RabbitMQ.Client;
using MediatR;
using NotificationService.Infrastructure.Configurations.SendGridConfiguration;
using NotificationService.Infrastructure.Services.EmailServices;
using NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Infrastructure.Services.RabbitMQServices;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.SigningKeys;
using System.Security.Cryptography;


namespace NotificationService.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMediatR(typeof(CreateNormalNotificationCommandHandler).Assembly);

            services.Configure<SendGridConfiguration>(configuration.GetSection("SendGrid"));
            services.AddSingleton<SendGridConfiguration>();
            services.AddScoped<EmailSenderContext>();

            // Email Strategies Registeration
            services.AddScoped<IWelcomeEmailStrategy, WelcomeEmailStrategy>();
            services.AddScoped<IConfirmEmailStrategy, ConfirmEmailStrategy>();
            services.AddScoped<IResetPasswordEmailStrategy, ResetPasswordEmailStrategy>();

            // Notification Strategies Registration
            services.AddScoped<IWelcomeNotificationStrategy, WelcomeNotificationStrategy>();
            services.AddScoped<IConfirmEmailNotificationStrategy, ConfirmEmailNotificationStrategy>();
            services.AddScoped<IResetPasswordStrategy, ResetPasswordStrategy>();

            // Notification Strategy Context Registration
            services.AddScoped<NotificationStrategyContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                RSA rsa = RSA.Create();
                rsa.ImportFromPem(SigningKeys.GetPublicKey());
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "Backend",
                    ValidAudience = "Frontend",
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

            services.AddHostedService<RabbitMQConsumerService>();

            return services;
        }
    }
}
