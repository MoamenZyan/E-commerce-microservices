using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.Features.Queries.GetCurrentUser;

namespace UserService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            GetCurrentUserQuery query = new GetCurrentUserQuery()
            {
                UserId = userId,
                Token = await HttpContext.GetTokenAsync("access_token"),
            };
            var result = await _mediator.Send(query);
            if (result != null)
                return Ok(result);
            return BadRequest();
        }
    }
}
