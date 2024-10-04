using MediatR;

namespace ReviewService.Application.Features.Commands.CreateReview
{
    public class CreateReviewCommand : IRequest<bool>
    {
        public required Guid ProductId { get; set; }
        public required Guid ReviewerId { get; set; }
        public string? Comment { get; set; }
        public required int Rate { get; set; }
    }
}
