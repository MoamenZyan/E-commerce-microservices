using MediatR;
using Microsoft.EntityFrameworkCore;
using ReviewService.Infrastructure.Data;
using Shared.Entities;

namespace ReviewService.Application.Features.Queries.GetProductsReviews
{
    public class GetProductsReviewsQueryHandler : IRequestHandler<GetProductsReviewsQuery, Dictionary<Guid, List<Review>>>
    {
        private readonly ApplicationDbContext _context;
        public GetProductsReviewsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<Guid, List<Review>>> Handle(GetProductsReviewsQuery request, CancellationToken cancellationToken)
        {
            var productsReviews = new Dictionary<Guid, List<Review>>();
            foreach(var productId in request.ProductsIds)
            {
                var reviews = await _context.Reviews.Where(x => x.ProductId == productId).ToListAsync();
                productsReviews.Add(productId, reviews);
            }

            return productsReviews;
        }
    }
}
