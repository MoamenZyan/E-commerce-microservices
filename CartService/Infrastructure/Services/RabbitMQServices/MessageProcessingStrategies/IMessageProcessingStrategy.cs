namespace CartService.Infrastructure.Services.RabbitMQServices.MessageProcessingStrategies
{
    public interface IMessageProcessingStrategy
    {
        Task<bool> Process(dynamic obj);
    }
}
