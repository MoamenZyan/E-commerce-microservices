using Microsoft.AspNetCore.Mvc;
using MediatR;
using CartService.Application.Features.Commands.AddProductToCart;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace CartService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("{productId}/{quantity}")]
        public async Task<IActionResult> AddProductToCart(Guid productId, int quantity)
        {
            AddProductToCartCommand command = new AddProductToCartCommand()
            {
                ProductId = productId,
                UserId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
                Email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value,
                Quantity = quantity
            };
            var result = await _mediator.Send(command);
            if (result)
                return NoContent();

            return BadRequest();
        }
    }
}
