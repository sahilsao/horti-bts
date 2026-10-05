using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.DDH;

public class SchemeTypeWiseNewApplicationsListDto
{
    public int? SchemeId { get; set; } // s_id
    public string? SchemeName { get; set; } // scheme_name
    public int? SchemeTypeId { get; set; } // st_id
    public int? TotalApplications { get; set; } // total_applications
    public int? RheoPending { get; set; } // 
    public int? RheoApproved { get; set; } // 
    public int? RheoRejected { get; set; } // 
    public int? DdhPending { get; set; } // 
    public int? DdhApproved { get; set; } // 
    public int? DdhRejected { get; set; } // 
}
