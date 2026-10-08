using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.DDH.OldApplication;

public class SchemeTypeWiseOldApplicationsListDto
{
    public int? SchemeId { get; set; }
    public string? SchemeName { get; set; } = string.Empty;
    public string? SchemeType { get; set; } = string.Empty;
    public int? SchemeTypeId { get; set; }
    public int? ApplicationCount { get; set; }
    public int? ApprovedApplicationCount { get; set; }
}
