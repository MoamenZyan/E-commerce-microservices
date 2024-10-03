using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderService.Application.Features.Commands.CreateOrder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using OrderService.Application.Features.Queries.GetAllUserOrders;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (token == null)
                return BadRequest("access token not exist");

            CreateOrderCommand command = new CreateOrderCommand()
            {
                UserId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
                Token = token
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest();

            return Ok(new {redirection_url = result});
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllUserOrders()
        {
            var userId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            GetAllUserOrdersQuery query = new GetAllUserOrdersQuery()
            {
                UserId = userId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
