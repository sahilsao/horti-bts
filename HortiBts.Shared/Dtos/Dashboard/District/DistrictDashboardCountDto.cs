namespace HortiBts.Shared.Dtos.Dashboard.District;

public record DistrictDashboardCountDto
{
    public int Count { get; set; }
}

public record DistrictApplicationDashboardDto
{
    public int TotalApplications { get; set; }
    public int TotalRheoApproved { get; set; }
    public int TotalDdhApproved { get; set; }
    public int TotalStateSponsored { get; set; }
    public int TotalCentralSponsored { get; set; }
}