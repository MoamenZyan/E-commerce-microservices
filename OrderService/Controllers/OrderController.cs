using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OrderService.Application.Features.Commands.CreateOrder;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using OrderService.Application.Features.Queries.GetAllUserOrders;
using Shared.Enums;
using OrderService.Application.Features.Commands.ConfirmOrder;
using System.Text;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateOrder([FromQuery] string paymentType)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (token == null)
                return BadRequest("access token not exist");

            CreateOrderCommand command = new CreateOrderCommand()
            {
                Email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value,
                UserId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value),
                Token = token,
                PaymentType = (PaymentType)Enum.Parse(typeof(PaymentType), paymentType) 
            };

            var result = await _mediator.Send(command);
            if (result == null)
                return BadRequest();

            return Ok(new {redirection_url = result});
        }

        [HttpGet]
        [Route("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpGet]
        [Route("success/{base64}")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmOrder(string base64)
        {
            byte[] decodedBytes = Convert.FromBase64String(base64);
            List<string> infos = Encoding.UTF8.GetString(decodedBytes).Split(":").ToList();

            ConfirmOrderCommand command = new ConfirmOrderCommand()
            {
                Email = infos[0],
                OrderId = new Guid(infos[1]),
            };

            var result = await _mediator.Send(command);
            if (result)
            {
                var script = "<script>window.close();</script>";
                return Content(script, "text/html");
            }

            return BadRequest(result);
        }
    }
}
