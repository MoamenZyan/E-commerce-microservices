using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentService.Application.Features.Commands.CreatePaymentOrder;
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
        public async Task<IActionResult> CreateCheckout([FromBody] CreateCheckoutCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _mediator.Send(command));
        }
    }
}
