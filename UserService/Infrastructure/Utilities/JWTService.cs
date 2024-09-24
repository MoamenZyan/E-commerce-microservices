using Shared.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Shared.SigningKeys;

namespace UserService.Infrastructure.Utilities
{
    public class JWTService
    {
        public static string GenerateJWTToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            RSA rsa = RSA.Create();
            rsa.ImportFromPem(SigningKeys.GetPrivateKey());

            var rsaSecurityKey = new RsaSecurityKey(rsa);

            var credentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256); ;


            var token = new JwtSecurityToken(
                claims: claims,
                issuer: "Backend",
                audience: "Frontend",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
