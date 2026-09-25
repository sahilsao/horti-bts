namespace HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

public class DistWiseFarmerBeneficiaryStatusDto
{
    public int? DistrictCode { get; set; } // district_code
	public string? DistrictNameEn { get; set; } // district_name
	public string? DistrictNameHi { get; set; } // district_name_hi
	public int? Approved { get; set; } // 
	public int? Rejected { get; set; } // 
}