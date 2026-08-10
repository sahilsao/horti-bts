using HortiBts.Api.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HortiBts.Api.Repositories.Auth
{
    internal interface IJwtTokenRepository
    {
        (string Token, DateTime ExpiresAtUtc, long LoginHistoryId) GenerateToken(LoginRecord user, string? ipAddress);
        (string token, DateTime expires) GenerateRefreshToken();
    }

    internal class JwtTokenRepository(IConfiguration config) : IJwtTokenRepository
    {
        public (string Token, DateTime ExpiresAtUtc, long LoginHistoryId) GenerateToken(LoginRecord user, string? ipAddress)
        {
            var jwtSection = config.GetSection("Jwt");

            var key = jwtSection["Key"]!;
            var issuer = jwtSection["Issuer"]!;
            var audience = jwtSection["Audience"]!;
            var expiryHours = double.Parse(jwtSection["ExpiryHours"] ?? "8");

            var expires = DateTime.UtcNow.AddHours(expiryHours);

            var loginHistoryId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserId),

                new(ClaimTypes.Role, GetRoleName(user.UserType)),

                new("username_en", user.UsernameEn),

                new("username_hi", user.UsernameHi),

                new("usertype", user.UserType.ToString()),

                new("lh_id", loginHistoryId.ToString()),

                new("district_code", user.DistrictCode ?? ""),

                new("password_flag", user.PasswordFlag ? "1" : "0"),
            };

            if (!string.IsNullOrWhiteSpace(ipAddress))
                claims.Add(new Claim("ip", ipAddress));

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: credentials);

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                expires,
                loginHistoryId);
        }
        public (string token, DateTime expires) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes);
            var expires = DateTime.UtcNow.AddDays(7);
            return (token, expires);
        }

        private static string GetRoleName(int userType)
        {
            return userType switch
            {
                13 => "Admin",
                14 => "District",
                3 => "RHEO",
                _ => "User"
            };
        }
    }
}



