using System;

namespace HortiBts.Shared.Dtos.Reports.CurrentFY.Applications;

public class VillageWiseFarmerApplicationsDto
{
   public int? FCount { get; set; } // 
	public int? SCount { get; set; } // 
	public decimal? Area { get; set; } // 
	public int? VillageCode { get; set; } // village_code
	public string? VillageName { get; set; } // village_name
	public int? OfficerCode { get; set; } // officer_code
	public string? OfficerName { get; set; } // name
	public string? MobileNo { get; set; } // mobile_no

	// this two needed for current fy
	public int? RApproved { get; set; } //  rheo_approved
	public int? DApproved { get; set; } //  ddh_approved

}
