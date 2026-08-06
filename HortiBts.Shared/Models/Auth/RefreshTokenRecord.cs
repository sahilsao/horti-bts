using HortiBts.Shared.Enums.Auth;

namespace HortiBts.Api.Models.Auth
{
    public class RefreshTokenRecord
    {
        public long Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public LoginType LoginType { get; set; }

        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedByIp { get; set; }

        public bool IsActive => DateTime.UtcNow < ExpiresAt;
    }
}
