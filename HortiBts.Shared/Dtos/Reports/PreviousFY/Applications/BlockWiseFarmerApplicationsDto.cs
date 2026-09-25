using System;

namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Applications;

public class BlockWiseFarmerApplicationsDto
{
	public int? FCount { get; set; } // 
	public int? SCount { get; set; } // 
	public decimal? Area { get; set; } // 
	public int? SubDistrictCode { get; set; } // subdistrict_code
	public string? SubDistrictNameHi { get; set; } // subdistrict_name_hi
	public string? SubDistrictNameEn { get; set; } // subdistrict_name_en
	public int? DistrictCode { get; set; } // district_code
	public string? DistrictNameHi { get; set; } // district_name_hi
	public string? DistrictNameEn { get; set; } // district_name_en

}
