namespace HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

public class FarmerWiseFarmerBeneficiaryStatusDto
{
	public int? UfId { get; set; } // uf_id
	public string? MaskAadharNumber { get; set; } // mask_aadhar_number
	public long? AadharNumber { get; set; } // aadhar_number
	public string? FarmerNameEng { get; set; } // farmer_name_eng
	public string? FarmerNameHi { get; set; } // farmer_name_hi
	public string? FatherName { get; set; } // father_name
	public string? Address { get; set; } // address
	public string? MaskMobileNo { get; set; } // mask_mobile_no
	public string? MobileNo { get; set; } // mobile_no
	public string? IfscCode { get; set; } // ifsc_code
	public string? MaskAccountNo { get; set; } // mask_account_no
	public string? AccountNo { get; set; } // account_no
	public int? VillageCode { get; set; } // village_code
	public string? Status { get; set; } // status
	public byte? StatusFlag { get; set; } // status_flag
	public string? Remark { get; set; } // remark
	public string? KhasraNo { get; set; } // khasra_no
	public decimal? Rakba { get; set; } // rakba
	public string? CropName { get; set; } // crop_name
}