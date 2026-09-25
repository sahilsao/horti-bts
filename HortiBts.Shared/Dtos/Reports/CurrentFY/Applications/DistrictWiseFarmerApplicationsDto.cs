using System;

namespace HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

public class DistrictWiseFarmerApplicationsDto
{
    public int? Target { get; set; } // 
	public int? FCount { get; set; } // 
	public int? SCount { get; set; } // 
	public decimal? Area { get; set; } // 	
	public int? DistrictCode { get; set; } // district_code
	public string? DistrictNameHi { get; set; } // district_name_hi
	public string? DistrictNameEn { get; set; } // district_name_en
	
	// this two needed for current fy
	public int? RApproved { get; set; } //  rheo_approved
	public int? DApproved { get; set; } //  ddh_approved
}
