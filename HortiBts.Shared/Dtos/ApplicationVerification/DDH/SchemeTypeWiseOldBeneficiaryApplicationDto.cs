using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.DDH;

public class SchemeTypeWiseOldBeneficiaryApplicationDto
{
    public int? SchemeId { get; set; }
    public string? SchemeName { get; set; } = string.Empty;
    public string? SchemeType { get; set; } = string.Empty;
    public int? SchemeTypeId { get; set; }
    public int? FarmerCount { get; set; }
    public int? ApprovedFarmerCount { get; set; }
}
