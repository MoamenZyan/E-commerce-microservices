using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using UserService.Application.Features.Commands.ClientSignup;
using UserService.Application.Features.Commands.ConfirmEmail;
using UserService.Application.Features.Commands.Login;
using UserService.Application.Features.Commands.ResetPassword;
using UserService.Application.Features.Commands.ResetPasswordRequest;
using UserService.Application.Features.Commands.Signup;
using UserService.Application.Features.Commands.VerifyEmail;

namespace UserService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register/admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] AdminSignupCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new {error = ex.Message});
            }
        }

        [HttpPost]
        [Route("register/client")]
        public async Task<IActionResult> RegisterClient([FromForm] ClientSignupCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromForm] LoginCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _mediator.Send(command);
                if (result.IsSuccess)
                    return Ok(result);

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("confirmEmail")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ConfirmEmail()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            ConfirmEmailCommand confirmEmailCommand = new ConfirmEmailCommand { UserId = userId };

            var result = await _mediator.Send(confirmEmailCommand);

            if (result == false)
                return BadRequest(new { error = "user not found / not authenticated" });

            return Ok();
        }

        [HttpGet]
        [Route("verifyEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] VerifyEmailCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            var result = await _mediator.Send(command);
            if (result == false)
                return BadRequest();


            var script = "<script>window.close();</script>";
            return Content(script, "text/html");
        }

        [HttpGet]
        [Route("resetPassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ResetPasswordRequest()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            ResetPasswordRequestCommand command = new ResetPasswordRequestCommand { UserId = userId };
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string userId, [FromQuery] string token)
        {
            var body = await Request.ReadFormAsync();
            ResetPasswordCommand command = new ResetPasswordCommand
            {
                UserId = userId,
                Token = token,
                Password = body["Password"]!,
            };

            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
