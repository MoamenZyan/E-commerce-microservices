﻿
using CartService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CartService.Infrastructure.Services.RabbitMQServices.MessageProcessingStrategies
{
    public class DeleteAllProductsFromCartStrategy : IMessageProcessingStrategy
    {
        private readonly ApplicationDbContext _context;
        public DeleteAllProductsFromCartStrategy(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Process(dynamic obj)
        {
            try
            {
                Guid userId = obj.content.UserId;
                var cart = await _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId);
                if (cart == null)
                    return true;

                await _context.ProductCarts.Where(x => x.CartId == cart.Id).ExecuteDeleteAsync();

                Log.Information($"cart has been cleared for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Log.Error(ex.Message);
                return false;
            }
        }
    }
}
