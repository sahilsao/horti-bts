namespace HortiBts.Api.Models.Auth
{
    public class PasswordPolicyResult
    {
        public bool MustChangePassword { get; set; }

        public bool PasswordExpired { get; set; }

        public DateTime? LastChanged { get; set; }
    }
}
