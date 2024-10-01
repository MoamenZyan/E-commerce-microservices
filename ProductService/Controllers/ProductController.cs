using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Commands.CreateProduct;
using ProductService.Application.Features.Commands.DeleteProduct;
using ProductService.Application.Features.Queries.GetAllProducts;
using ProductService.Application.Features.Queries.GetAllUserProducts;
using ProductService.Application.Features.Queries.GetProduct;
using ProductService.Application.Features.Queries.GetProductsInfo;
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

        [HttpGet]
        [Route("all/{userId}")]
        public async Task<IActionResult> GetUserProducts(Guid userId)
        {
            GetAllUserProductsQuery query = new GetAllUserProductsQuery()
            {
                UserId = userId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("info")]
        public async Task<IActionResult> GetProductsInfo([FromBody] List<Guid> productsIds)
        {
            GetProductsInfoQuery query = new GetProductsInfoQuery()
            {
                ProductsIds = productsIds
            };
            var result = await _mediator.Send(query);
            return Ok(result);
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

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            GetAllProductsQuery query = new GetAllProductsQuery();
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete]
        [Route("{productId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (token == null)
                return BadRequest("access token not exist");

            DeleteProductCommand command = new DeleteProductCommand()
            {
                ProductId = productId,
                UserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value,
                Token = token
            };

            var result = await _mediator.Send(command);
            if (result) 
                return NoContent();

            return BadRequest();
        }
    }
}
