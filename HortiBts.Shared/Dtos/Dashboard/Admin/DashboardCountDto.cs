namespace HortiBts.Shared.Dtos.Dashboard.Admin;

public record DashboardCountDto
{
    public int Count { get; set; }
}

public record ApplicationDashboardDto
{
    public int TotalApplications { get; set; }
    public int TotalRheoApproved { get; set; }
    public int TotalDdhApproved { get; set; }
    public int TotalStateSponsored { get; set; }
    public int TotalCentralSponsored { get; set; }
}