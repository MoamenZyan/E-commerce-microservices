using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Entities;
using UserService.Application.DTOs;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.Services;

namespace UserService.Application.Features.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto?>
    {
        private readonly ExternalHttpService _httpService;
        private readonly UserManager<ApplicationUser> _manager;
        public GetCurrentUserQueryHandler(UserManager<ApplicationUser> manager, ExternalHttpService httpService)
        {
            _httpService = httpService;
            _manager = manager;
        }
        public async Task<UserDto?> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _manager.FindByIdAsync(request.UserId);
            if (user == null)
                return null;

            var ownedProduct = await _httpService.GetUserProducts(new Guid(user.Id));
            var cart = await _httpService.GetUserCart(new Guid(user.Id), request.Token);

            List<dynamic>? productsCartInfo = new List<dynamic>();

            if (cart != null)
            {
                productsCartInfo = await _httpService.GetProductsInfo(cart.Products.Select(x => x.ProductId).ToList());
            }

            UserDto userDto = new UserDto()
            {
                Id = new Guid(user.Id),
                UserName = user.UserName!,
                Email = user.Email!,
                Cart = cart != null ? new {CartId = cart.Id,
                          CreatedAt = cart.CreatedAt, CartProducts = productsCartInfo } : null,
                OwnedProducts = ownedProduct
            };
            
            return userDto;
        }
    }
}
