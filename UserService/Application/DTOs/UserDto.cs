using Shared.Entities;

namespace UserService.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public dynamic? Cart { get; set; } = null!;
        public List<Product>? OwnedProducts { get; set; } = new List<Product>();
    }
}
