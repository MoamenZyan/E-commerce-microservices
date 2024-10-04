using MediatR;
using PaymentService.Application.Responses;
using Shared.DTOs;
using Shared.Entities;
using Shared.Enums;

namespace PaymentService.Application.Features.Commands.CreatePaymentOrder
{
    public class CreateCheckoutCommand : IRequest<CheckoutResponse>
    {
        public required List<ProductItemDto> Products { get; set; }
        public required string Email { get; set; }
        public required string PaymentType { get; set; }
    }
}
