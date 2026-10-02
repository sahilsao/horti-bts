using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.DDH;

public class SchemeWiseApplicationsListDto
{
    public int? SchemeId { get; set; } // s_id
    public string? SchemeName { get; set; } // scheme_name
    public int? SchemeTypeId { get; set; } // st_id
    public int? TotalApplications { get; set; } // 
    public int? RheoApproved { get; set; } // 
    public int? DdhApproved { get; set; } // 
}
