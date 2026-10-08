using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.DDH.NewApplication;

public class SchemeWiseNewBeneficiaryApplicationListDto
{
    public int? ApplicationId { get; set; } // application_id
    public int? HfId { get; set; } // hf_id
    public int? SdId { get; set; } // sd_id
    public int? UfId { get; set; } // uf_id
    public string? FarmerNameEng { get; set; } // farmer_name_eng
    public string? FarmerNameHi { get; set; } // farmer_name_hi
    public string? FatherName { get; set; } // father_name
    public string? MobileNo { get; set; } // mobile_no
    public int? SchemeType { get; set; } // scheme_type
    public int? SchemeId { get; set; } // scheme_id
    public string? SchemeName { get; set; } // scheme_name
    public string? ComponentName { get; set; } // cname
    public int? ComponentId { get; set; } // component_id
    public decimal? Area { get; set; } // area
    public int? Quantity { get; set; } // quantity
    public int? FinancialYear { get; set; } // financial_year
    public string? IfscCode { get; set; } // ifsc_code
    public string? AccountNo { get; set; } // account_no
    public string? TotalLandKhasra { get; set; } // total_land_khasra
    public decimal? TotalLandArea { get; set; } // total_land_area
    public string? KhasraNo { get; set; } // khasra_no
    public string? Status { get; set; } // status
    public DateTime? RegDate { get; set; } // created_at
    public int? VillageCode { get; set; } // village_code
    public string? VillageName { get; set; } // village_name
}
