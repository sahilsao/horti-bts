using System;

namespace HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

public class RHEOWiseFarmerApplicationsDto
{
   public int? FCount { get; set; } // 
	public int? SCount { get; set; } // 
	public decimal? Area { get; set; } // 
	public int? OfficerCode { get; set; } // officer_code
	public string? OfficerName { get; set; } // officer_name
	public string? MobileNo { get; set; } // mobile_no
	public int? SubDistrictCode { get; set; } // subdistrict_code
	public string? SubDistrictNameHi { get; set; } // subdistrict_name_hi
	public string? SubDistrictNameEn { get; set; } // subdistrict_name_en
	public int? DistrictCode { get; set; } // district_code
	public string? DistrictNameHi { get; set; } // district_name_hi
	public string? DistrictNameEn { get; set; } // district_name_en

	// this two needed for current fy
	public int? RApproved { get; set; } //  rheo_approved
	public int? DApproved { get; set; } //  ddh_approved
}

