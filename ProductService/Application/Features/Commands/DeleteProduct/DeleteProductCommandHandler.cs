﻿
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Services.ExternalHttpServices;
using Shared.Entities;

namespace ProductService.Application.Features.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ExternalHttpService _externalHttpService;
        public DeleteProductCommandHandler(ApplicationDbContext context, ExternalHttpService externalHttpService)
        {
            _context = context;
            _externalHttpService = externalHttpService;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var roles = await _externalHttpService.GetUserRoles(request.UserId, request.Token);

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId);
            if (product == null) 
                return true;

            if (product.OwnerId == new Guid(request.UserId) || (roles != null && roles.Contains("Admin")))
            {
                OutboxMessage message = new OutboxMessage()
                {
                    Id = Guid.NewGuid(),
                    MessageType = Shared.Enums.MessageTypes.ProductDeleted,
                    Content = JsonConvert.SerializeObject(product),
                    Processed = false,
                    CreatedAt = DateTime.Now,
                };

                _context.Products.Remove(product);
                await _context.OutboxMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
