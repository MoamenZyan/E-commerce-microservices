using MediatR;
using ReviewService.Infrastructure.Data;
using Serilog;
using Shared.Entities;

namespace ReviewService.Application.Features.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        public CreateReviewCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            Review review = new Review()
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Comment = request.Comment,
                ProductId = request.ProductId,
                ReviewerId = request.ReviewerId,
                Rate = request.Rate,
            };

            try
            {
                await _context.Reviews.AddAsync(review);
                await _context.SaveChangesAsync();

                Log.Information($"user {request.ReviewerId} has reviewed product {request.ProductId}");
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }
    }
}
