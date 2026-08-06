namespace HortiBts.Api.Helpers;

public static class IpAddressHelper
{
    public static string GetClientIp(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;

        if (context is null)
            return "Unknown";

        var forwarded = context.Request.Headers["X-Forwarded-For"]
            .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(forwarded))
            return forwarded.Split(',')[0].Trim();

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}