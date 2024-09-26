using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Services;
using RabbitMQ;
using RabbitMQ.Client;
using MediatR;
using NotificationService.Infrastructure.Configurations.SendGridConfiguration;
using NotificationService.Infrastructure.Services.EmailServices;
using NotificationService.Application.EmailStrategies;
using NotificationService.Infrastructure.Services.EmailServices.StrategiesImplementation;
using NotificationService.Application.Features.Command.CreateNormalNotification;
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
            services.AddScoped<WelcomeEmailStrategy>();


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
