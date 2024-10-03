using MediatR;
using PaymentService.Application.Responses;
using Shared.DTOs;
using Shared.Entities;
using Shared.Enums;

namespace PaymentService.Application.Features.Commands.CreatePaymentOrder
{
    public class CreatePaymentOrderCommand : IRequest<OrderPaymentCreationResponse>
    {
        public decimal Total { get; set; }
        public Guid PayerId { get; set; }
        public required List<ProductItemDto> Products { get; set; }
        public string? PaymentType { get; set; }
    }
}
