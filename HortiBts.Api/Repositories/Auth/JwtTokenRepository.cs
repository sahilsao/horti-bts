using HortiBts.Api.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HortiBts.Api.Repository.Auth
{
    internal interface IJwtTokenRepository
    {
        (string Token, DateTime ExpiresAtUtc) GenerateToken(LoginRecord user, string? ipAddress);
        (string token, DateTime expires) GenerateRefreshToken();
    }

    internal class JwtTokenRepository(IConfiguration config) : IJwtTokenRepository
    {
        public (string Token, DateTime ExpiresAtUtc) GenerateToken(LoginRecord user, string? ipAddress)
        {
            var jwtSection = config.GetSection("Jwt");
            var key = jwtSection["Key"]!;
            var issuer = jwtSection["Issuer"]!;
            var audience = jwtSection["Audience"]!;
            var expiryHours = double.Parse(jwtSection["ExpiryHours"] ?? "8");

            var role = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(user.Role.ToLower());
            var expires = DateTime.UtcNow.AddHours(expiryHours);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserId),
                new(ClaimTypes.Role, role),
                new("username_en", user.UsernameEn ?? ""),
                new("username_hi", user.UsernameHi ?? ""),
            };
            if (!string.IsNullOrEmpty(ipAddress))
                claims.Add(new Claim("ip", ipAddress));

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
        public (string token, DateTime expires) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes);
            var expires = DateTime.UtcNow.AddDays(7);
            return (token, expires);
        }
    }
}
