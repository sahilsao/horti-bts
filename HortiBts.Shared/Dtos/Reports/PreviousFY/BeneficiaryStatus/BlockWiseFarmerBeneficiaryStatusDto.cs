namespace HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

public class BlockWiseFarmerBeneficiaryStatusDto
{
	public int? SubDistrictCode { get; set; }
	public string? SubDistrictNameEn { get; set; } // subDistrict_name
	public string? SubDistrictNameHi { get; set; } // subDistrict_name_hi
	public int? Approved { get; set; } // 
	public int? Rejected { get; set; } // 
}