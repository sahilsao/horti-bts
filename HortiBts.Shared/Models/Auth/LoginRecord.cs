namespace HortiBts.Api.Models.Auth
{
    public class LoginRecord
    {
        public string UserId { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string UsernameEn { get; set; } = string.Empty;

        public string UsernameHi { get; set; } = string.Empty;

        public int UserType { get; set; }

        public bool PasswordFlag { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? DistrictCode { get; set; }

        public int? SubDistrictCode { get; set; }
    }
}
