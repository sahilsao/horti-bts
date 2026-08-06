using HortiBts.Shared.Enums.Auth;
using System.ComponentModel.DataAnnotations;

namespace HortiBts.Client.Pages.Auth.Models
{
    public class LoginModel
    {
        [Required]
        public LoginType LoginType { get; set; } = LoginType.Admin;

        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Provide User Password | कृपया उपयोगकर्ता पासवर्ड प्रदान करें")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string? UsernameEn { get; set; }

        public string? UsernameHi { get; set; }
    }
}

