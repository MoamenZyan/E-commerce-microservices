
using CartService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Entities;
using System.Text;

namespace CartService.Infrastructure.Services.RabbitMQServices
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private object _lock = new object();
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        public RabbitMQConsumerService(IConnection connection, IModel channel, IServiceProvider services)
        {
            _channel = channel;
            _connection = connection;
            _services = services;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //if (!stoppingToken.IsCancellationRequested)
            //{
            //    var consumer = new AsyncEventingBasicConsumer(_channel);
            //    consumer.Received += ProcessMessage;
            //
            //    _channel.BasicConsume("product", false, consumer);
            //}
            return Task.CompletedTask;
        }

    }
}
