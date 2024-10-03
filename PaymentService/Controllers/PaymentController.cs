using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentService.Application.Features.Commands.CreatePaymentOrder;
using PaymentService.Application.Features.Queries.CheckOrder;
using Shared.Enums;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = "APIKey")]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentOrder([FromBody] CreatePaymentOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> CheckOrder([FromQuery] CheckOrderQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }
    }
}
