using System.ComponentModel.DataAnnotations;

namespace HortiBts.Client.Pages.Auth.Models
{
    public class LoginModel
    {
        [Required]
        public string LoginType { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Provide User Id | कृपया उपयोगकर्ता आईडी प्रदान करें")]
        public string UserId { get; set; } = string.Empty;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Provide User Password | कृपया उपयोगकर्ता पासवर्ड प्रदान करें")]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string? Username { get; set; }
        public string? UsernameEn { get; set; }
        public string? UsernameHi { get; set; }
    }
}
