namespace HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

public class VillageWiseFarmerBeneficiaryStatusDto
{
	public int? VillageCode { get; set; }
	public string? VillageNameEn { get; set; }
	public string? VillageNameHi { get; set; }
	public int? Approved { get; set; } // 
	public int? Rejected { get; set; } // 
}