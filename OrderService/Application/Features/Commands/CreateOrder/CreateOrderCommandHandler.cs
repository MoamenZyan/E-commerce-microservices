using MediatR;
using Newtonsoft.Json;
using OrderService.Domain;
using OrderService.Infrastructure.Data;
using OrderService.Infrastructure.Services.ExternalHttpServices;
using Serilog;
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
            if (productsInfo == null || productsInfo.Count == 0)
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

            var obj = new
            {
                Products = products,
                PaymentType = request.PaymentType.ToString(),
                Email = request.Email
            };
            
            Checkout result = await _externalHttpService.CreateCheckout(obj);

            if (!result.IsSuccess)
                return null;
  
            Order order = new Order()
            {
                Id = result.OrderId,
                ExternalId = result.CheckoutId!,
                Status = OrderStatus.PENDING,
                Total = products.Sum(x => x.Price * x.Quantity),
                UserId = request.UserId,
                IssuedAt = DateTime.Now,
                PaymentType = request.PaymentType
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

            Log.Information($"order has been issued for user {request.UserId} of payment type {request.PaymentType.ToString()}");
            return result.RedirectionUrl;
        }
    }
}
