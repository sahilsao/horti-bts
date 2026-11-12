using System;

namespace HortiBts.Shared.Dtos.AddBeneficiaryGrant;

public class ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto
{
    public int? ApplicationId { get; set; }
    public int? HfId { get; set; }
    public int? SdId { get; set; }
    public int? UfId { get; set; }
    public string? FarmerNameEng { get; set; }
    public string? FarmerNameHi { get; set; }
    public string? FatherName { get; set; }
    public string? MobileNo { get; set; }
    public int? SchemeType { get; set; }
    public int? SchemeId { get; set; }
    public string? SchemeName { get; set; }
    public string? ComponentName { get; set; } // component_name
    public int? ComponentId { get; set; }
    public decimal? Area { get; set; }
    public int? Quantity { get; set; }
    public int? FinancialYear { get; set; }
    public int? VillageCode { get; set; }
    public string? IfscCode { get; set; }
    public string? AccountNo { get; set; }
    public string? TotalLandKhasra { get; set; }
    public decimal? TotalLandArea { get; set; }
    public string? KhasraNo { get; set; }
    public string? Status { get; set; }
    public DateTime RegDate { get; set; } // reg_date
    public string? VillageName { get; set; }
}
