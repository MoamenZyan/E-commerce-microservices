
using CartService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CartService.Infrastructure.Services.RabbitMQServices.MessageProcessingStrategies
{
    public class DeleteProductFromCartStrategy : IMessageProcessingStrategy
    {
        private readonly ApplicationDbContext _context;
        public DeleteProductFromCartStrategy(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Process(dynamic obj)
        {
            try
            {
                var productId = new Guid(obj.content.productId);
                var product = await _context.ProductCarts.FirstOrDefaultAsync(x => x.ProductId == productId);
                if (product != null)
                    _context.ProductCarts.Remove(product);

                await _context.SaveChangesAsync();

                Log.Information($"product {product!.ProductId} has been removed from user {product.Cart.UserId}");
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
