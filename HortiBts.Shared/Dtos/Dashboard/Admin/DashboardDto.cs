namespace HortiBts.Shared.Dtos.Dashboard.Admin;

public record DashboardDto
{
    public int TotRHEO { get; set; }
    public int TotRegFarmers { get; set; }
    public int TotRegBacklogFarmers { get; set; }
    public int TotApplications { get; set; }
    public int TotApprovedFarmersRHEO { get; set; }
    public int TotApprovedFarmersDDH { get; set; }
    public int TotApplicationsCentralSponsoredScheme { get; set; }
    public int TotApplicationsStateSponsoredScheme { get; set; }
}

