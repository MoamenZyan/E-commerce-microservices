using CartService.Domain.Entities;
using CartService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using Shared.Entities;

namespace CartService.Application.Features.Commands.AddProductToCart
{
    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public AddProductToCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {

            var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == request.UserId);
            if (cart == null)
            {
                cart = new Cart()
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    CreatedAt = DateTime.Now,
                };

                await _context.Carts.AddAsync(cart);
            }

            ProductCart productCart = new ProductCart()
            {
                ProductId = request.ProductId,
                CartId = cart.Id,
                Quantity = request.Quantity
            };

            OutboxMessageCart message = new OutboxMessageCart()
            {
                Id = Guid.NewGuid(),
                Processed = false,
                CreatedAt = DateTime.Now,
                MessageType = Shared.Enums.MessageTypes.ProductAddedToCart,
                Content = JsonConvert.SerializeObject(request),
                IsNotification = true
            };

            try
            {
                await _context.ProductCarts.AddAsync(productCart);
                await _context.OutboxMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                Log.Information($"User {request.UserId} has added product {request.ProductId} to his cart");
                return true;
            }
            catch (Exception)
            {
                Log.Error($"Error in adding product {request.ProductId} in user {request.UserId} cart");
                return false;
            }
        }
    }
}
