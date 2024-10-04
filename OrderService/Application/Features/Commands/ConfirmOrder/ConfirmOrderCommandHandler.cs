using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services.ExternalHttpServices;
using Serilog;
using Shared.Entities;

namespace OrderService.Application.Features.Commands.ConfirmOrder
{
    public class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ExternalHttpService _externalHttpService;
        public ConfirmOrderCommandHandler(ApplicationDbContext context, ExternalHttpService externalHttpService)
        {
            _context = context;
            _externalHttpService = externalHttpService;
        }
        public async Task<bool> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == request.OrderId);
            if (order == null)
                return false;

            order.Status = Shared.Enums.OrderStatus.COMPLETED;


            var productsInfo = await _externalHttpService.GetProductsInfo(order.Items.Select(x => x.ProductId).ToList());
            if (productsInfo == null)
                return false;

            var products = new List<dynamic>();
            foreach (var product in productsInfo)
            {
                var obj = new
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = order.Items.First(x => x.ProductId == product.Id).Quantity,
                };
                products.Add(obj);
            }
            

            var content = new
            {
                UserId = order.UserId,
                Email = request.Email,
                OrderId = order.Id,
                Total = order.Total,
                Products = products,
                PaymentType = order.PaymentType.ToString()
            };

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            OutboxMessage message1 = new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(content, settings),
                CreatedAt = DateTime.Now,
                Processed = false,
                MessageType = Shared.Enums.MessageTypes.OrderConfirmed
            };

            OutboxMessage message2 = new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                Content = JsonConvert.SerializeObject(new {UserId = order.UserId}),
                CreatedAt = DateTime.Now,
                Processed = false,
                MessageType = Shared.Enums.MessageTypes.ClearCart
            };

            await _context.OutboxMessages.AddAsync(message1);
            await _context.OutboxMessages.AddAsync(message2);
            _context.Update(order);
            await _context.SaveChangesAsync();

            Log.Information($"order {order.Id} has been confirmed");
            return true;
        }
    }
}
