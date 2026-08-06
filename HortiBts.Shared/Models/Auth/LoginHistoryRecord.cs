namespace HortiBts.Api.Models.Auth
{
    public class LoginHistoryRecord
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; } = string.Empty;

        public long LoginHistoryId { get; set; }

        public string AuthToken { get; set; } = string.Empty;

        public DateTime TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public string? IpAddress { get; set; }

        public int Status { get; set; }

        public string? BrowserVersion { get; set; }
    }
}
