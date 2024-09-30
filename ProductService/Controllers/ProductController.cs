using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands.CreateProduct;
using ProductService.Application.Features.Queries.GetProduct;
using System.Security.Claims;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct([FromQuery] GetProductQuery query)
        {
            var result = await _mediator.Send(query);
            if (result != null)
                return Ok(result);

            return NotFound();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostProduct([FromForm] CreateProductCommand command)
        {
            command.OwnerId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            command.Email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;

            var result = await _mediator.Send(command);
            if (result)
                return NoContent();

            return BadRequest();
        }
    }
}
