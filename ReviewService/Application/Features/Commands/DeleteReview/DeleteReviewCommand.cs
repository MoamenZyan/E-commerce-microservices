using MediatR;

namespace ReviewService.Application.Features.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public required Guid ReviewerId { get; set; }
        public required Guid ReviewId { get; set; }
    }
}
