using System.ComponentModel.DataAnnotations;

namespace HortiBts.Client.Pages.Auth.Models
{
    public class PasswordModel
    {
        public string UserId { get; set; } = string.Empty;
        public string UsernameEn { get; set; } = string.Empty;
        public string UsernameHi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Current password is required. | वर्तमान पासवर्ड आवश्यक है.")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required. | नया पासवर्ड आवश्यक है.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters. | पासवर्ड कम से कम 8 अक्षरों का होना चाहिए.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your new password. | कृपया अपना नया पासवर्ड पुष्टि करें.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match. | पासवर्ड मेल नहीं खाते।")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
