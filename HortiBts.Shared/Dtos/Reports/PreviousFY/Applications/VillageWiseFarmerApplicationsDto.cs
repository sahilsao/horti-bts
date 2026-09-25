using System;

namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Applications;

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


}
