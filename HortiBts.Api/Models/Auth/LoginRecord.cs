namespace HortiBts.Api.Models.Auth
{
    internal sealed class LoginRecord
    {
        public string UserId { get; set; } = "";
        public string Password { get; set; } = ""; // BCrypt hash
        public string Role { get; set; } = "";
        public string UsernameEn { get; set; } = "";
        public string UsernameHi { get; set; } = "";
    }
}
