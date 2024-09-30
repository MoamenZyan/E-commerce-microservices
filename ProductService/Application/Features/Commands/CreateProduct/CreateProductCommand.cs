using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Enums;

namespace ProductService.Application.Features.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<bool>
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        [BindNever]
        public string? Email { get; set; }
        public CategoryTypes Category { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
    }
}
