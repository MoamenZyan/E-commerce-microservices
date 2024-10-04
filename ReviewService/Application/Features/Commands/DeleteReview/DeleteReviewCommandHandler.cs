using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Infrastructure.Data;
using Serilog;

namespace ReviewService.Application.Features.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public DeleteReviewCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(x => x.Id == request.ReviewId);
            if (review == null)
                return true;

            if (review.ReviewerId == request.ReviewerId)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                Log.Information($"user {request.ReviewerId} deleted his review on product {review.ProductId}");
                return true;
            }
            else
            {
                Log.Warning($"user {request.ReviewerId} tried to delete someone's review !!");
                return false;
            }
        }
    }
}
