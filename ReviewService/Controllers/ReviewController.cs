using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Application.Features.Commands.CreateReview;
using ReviewService.Application.Features.Commands.DeleteReview;
using ReviewService.Application.Features.Queries.GetAllProductReviews;
using ReviewService.Application.Features.Queries.GetProductsReviews;
using System.Security.Claims;

namespace ReviewService.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetAllReviewsOfProduct(string productId)
        {
            GetAllProductReviewsQuery query = new GetAllProductReviewsQuery()
            {
                ProductId = new Guid(productId),
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [Route("all")]
        public async Task<IActionResult> GetProductsReviews([FromBody] GetProductsReviewsQuery query)
        {
            var products = await _mediator.Send(query);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateReview()
        {
            Guid userId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var body = await Request.ReadFormAsync();

            try
            {
                CreateReviewCommand command = new CreateReviewCommand()
                {
                    ReviewerId = userId,
                    ProductId = new Guid(body["ProductId"]!),
                    Rate = Convert.ToInt32(body["Rate"]!),
                    Comment = body["comment"]
                };

                var result = await _mediator.Send(command);
                if (result)
                    return NoContent();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("{reviewId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteReview(string reviewId)
        {
            var userId = new Guid(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            DeleteReviewCommand command = new DeleteReviewCommand()
            {
                ReviewId = new Guid(reviewId),
                ReviewerId = userId
            };

            var result = await _mediator.Send(command);
            if (result)
                return NoContent();

            return BadRequest();
        }

    }
}
