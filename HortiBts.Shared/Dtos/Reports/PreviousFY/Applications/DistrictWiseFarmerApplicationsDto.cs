using System;

namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Applications;

public class DistrictWiseFarmerApplicationsDto
{
    public int? Target { get; set; } // 
	public int? FCount { get; set; } // 
	public int? SCount { get; set; } // 
	public decimal? Area { get; set; } // 	
	public int? DistrictCode { get; set; } // district_code
	public string? DistrictNameHi { get; set; } // district_name_hi
	public string? DistrictNameEn { get; set; } // district_name_en
	
}
