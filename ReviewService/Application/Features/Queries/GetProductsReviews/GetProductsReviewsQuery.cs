using MediatR;
using Shared.Entities;

namespace ReviewService.Application.Features.Queries.GetProductsReviews
{
    public class GetProductsReviewsQuery : IRequest<Dictionary<Guid, List<Review>>>
    {
        public required List<Guid> ProductsIds { get; set; }
    }
}
