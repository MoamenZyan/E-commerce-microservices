
using MediatR;
using Shared.Entities;

namespace ReviewService.Application.Features.Queries.GetAllProductReviews
{
    public class GetAllProductReviewsQuery : IRequest<List<Review>?>
    {
        public required Guid ProductId { get; set; }
    }
}
