using System;

namespace HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

public class FarmerWiseFarmerApplicationsDto
{
    public string? ApplicationId { get; set; }
    public string? UfId { get; set; }

    public int FdId { get; set; }

    public int HfId { get; set; }

    public int? SdId { get; set; }

    public int VillageCode { get; set; }

    public string? VillageName { get; set; }

    public string? FarmerNameEng { get; set; }

    public string? FarmerNameHi { get; set; }

    public string? FatherName { get; set; }

    public string? MobileNo { get; set; }

    public int? SchemeType { get; set; }

    public int? SchemeId { get; set; }

    public string? SchemeName { get; set; }

    public string? ComponentName { get; set; }

    public int? ComponentId { get; set; }

    public decimal? Area { get; set; }

    public decimal? Quantity { get; set; }

    public string? KhasraNo { get; set; }

    public string? TotalLandKhasra { get; set; }

    public decimal? TotalLandArea { get; set; }
}
