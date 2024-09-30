using MediatR;
using Newtonsoft.Json;
using ProductService.Infrastructure.Data;
using Serilog;
using Shared.Entities;

namespace ProductService.Application.Features.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public CreateProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Product product = new Product()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = request.OwnerId,
                    Name = request.Name,
                    Category = request.Category,
                    Description = request.Description,
                    Price = request.Price,
                    Discount = request.Discount,
                    CreatedAt = DateTime.Now,
                };

                OutboxMessage message = new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    Content = JsonConvert.SerializeObject(new { product, Email = request.Email }),
                    CreatedAt = DateTime.Now,
                    MessageType = Shared.Enums.MessageTypes.ProductCreated
                };

                try
                {
                    await _context.Products.AddAsync(product);
                    await _context.OutboxMessages.AddAsync(message);
                    await _context.SaveChangesAsync();
                    Log.Information($"User {request.OwnerId} created product {product.Id}");
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Error($"Failed to create product {product.Id} for user {request.OwnerId}");
                    Console.WriteLine(ex);
                    return false;
                }
            }
            return false;
        }
    }
}
