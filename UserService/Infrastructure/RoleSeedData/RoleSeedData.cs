using Microsoft.AspNetCore.Identity;

namespace UserService.Infrastructure.RoleSeedData
{
    public static class RoleSeedData
    {
        public static async Task SeedData(RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string>()
            {
                "Admin", "Client"
            };

            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
