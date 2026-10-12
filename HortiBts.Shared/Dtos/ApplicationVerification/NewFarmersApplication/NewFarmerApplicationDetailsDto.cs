using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication;

public class NewFarmerApplicationDetailsDto
{
    public int? ApplicationId { get; set; } // application_id
    public int? HfId { get; set; } // hf_id
    public int? FdId { get; set; } // fd_id
    public int? BdId { get; set; } // bd_id
    public int? Status { get; set; } // status
    public string? FarmerNameEng { get; set; } // farmer_name_eng
    public string? FarmerNameHi { get; set; } // farmer_name_hi
    public int? FinancialYear { get; set; } // financial_year
    public int? RheoApprovalStatus { get; set; } // rheo_approval_status
    public int? DdhApprovalStatus { get; set; } // ddh_approval_status
    public string? RheoRemark { get; set; } // rheo_remark
    public string? DdhRemark { get; set; } // ddh_remark
    public string? Remark { get; set; } // remark
}
