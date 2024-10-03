using MediatR;
using Newtonsoft.Json;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services.ExternalHttpServices;
using Shared.DTOs;
using Shared.Entities;
using Shared.Enums;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, string?>
    {
        private readonly ExternalHttpService _externalHttpService;
        private readonly ApplicationDbContext _context;
        public CreateOrderCommandHandler(ExternalHttpService externalHttpService, ApplicationDbContext context)
        {
            _externalHttpService = externalHttpService;
            _context = context;
        }
        public async Task<string?> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _externalHttpService.GetUserCart(request.UserId, request.Token);
            if (cart == null)
                return null;

            var productsInfo = await _externalHttpService.GetProductsInfo(cart.Products.Select(x => x.ProductId).ToList());
            if (productsInfo == null)
                return null;


            

            var products = new List<ProductItemDto>();
 
            foreach (var product in productsInfo)
            {
                var item = new ProductItemDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = Convert.ToString(product.Category),
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = cart.Products.First(x => x.ProductId == product.Id).Quantity
                };
                products.Add(item);
            }

            decimal total = products.Sum(x => x.Price * x.Quantity);

            var obj = new
            {
                Total = total,
                Products = products,
                PayerId = request.UserId,
                PaymentType = PaymentType.Paypal.ToString(),
            };
            
            var result = await _externalHttpService.CreatePaypalOrder(obj);

            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                ExternalId = result.Id,
                Status = (OrderStatus)Enum.Parse(typeof(OrderStatus), result.Status),
                Total = total,
                UserId = request.UserId,
                IssuedAt = DateTime.Now,
            };

            var orderItems = new List<OrderItem>();
            foreach (var item in products)
            {
                var orderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = item.Id,
                    Quantity = item.Quantity,
                };
                orderItems.Add(orderItem);
            }

            await _context.Orders.AddAsync(order);
            await _context.Items.AddRangeAsync(orderItems);
            await _context.SaveChangesAsync();

            return result.Links.First(x => x.Rel == "approve").Href;
        }
    }
}
