using MediatR;
using Newtonsoft.Json;
using NotificationService.Application.Features.Command.CreateNormalNotification;
using NotificationService.Application.Interfaces.EmailStrategies;
using NotificationService.Application.Interfaces.NotificationStrategies;
using NotificationService.Infrastructure.Services.EmailServices;

namespace NotificationService.Infrastructure.Services.RabbitMQServices.StrategiesImplementation
{
    public class OrderConfirmedNotificationStrategy : IOrderConfirmedNotificationStrategy
    {
        private readonly IServiceProvider _serviceProvider;
        public OrderConfirmedNotificationStrategy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Process(dynamic messageObj)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var emailSenderContext = scope.ServiceProvider.GetRequiredService<EmailSenderContext>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var orderConfirmedEmailStrategy = scope.ServiceProvider.GetRequiredService<IOrderConfirmedEmailStrategy>();

                    emailSenderContext.SetStrategy(orderConfirmedEmailStrategy);

                    List<string> orderDetail = new List<string>();
                    foreach (dynamic product in messageObj.Products)
                    {
                        string productDetail = $@"
                             <p>
                                 <strong>Name:</strong> {product.Name}<br>
                                 <strong>Description:</strong> {product.Description}.<br>
                                 <strong>Price:</strong> {Convert.ToDecimal(product.Price)}$<br>
                                 <strong>Quantity:</strong> {product.Quantity}
                             </p>
                        ";

                        orderDetail.Add(productDetail);
                    };

                    var notificationBody = $@"
                                    Your order <strong>{messageObj.OrderId}<strong/> has been confirmed.
                                    <h3>Order Details:-</h3>
                                    <p><strong>Payment Type:</strong> {messageObj.PaymentType}</p>
                                    <div>{string.Join("\n", orderDetail)}</div>
                                    <br>
                                    <h2>Order Total: {messageObj.Total}$</h2>
                    ";


                    CreateNormalNotificationCommand createNormalNotificationCommand = new CreateNormalNotificationCommand()
                    {
                        UserId = messageObj.UserId,
                        Body = $"your order {messageObj.OrderId} has been confirmed."
                    };

                    await mediator.Send(createNormalNotificationCommand);
                    await emailSenderContext.Send("", Convert.ToString(messageObj.Email), notificationBody);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
