using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Infrastructure.Data;
using Shared.Entities;

namespace ReviewService.Application.Features.Queries.GetAllProductReviews
{
    public class GetAllProductReviewsQueryHandler : IRequestHandler<GetAllProductReviewsQuery, List<Review>?>
    {
        private readonly ApplicationDbContext _context;
        public GetAllProductReviewsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Review>?> Handle(GetAllProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.Reviews.Where(x => x.ProductId == request.ProductId).ToListAsync();
            return reviews;
        }
    }
}
