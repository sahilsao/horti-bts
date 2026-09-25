using System;

namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Yearly;

public class FarmerWiseFarmerRegistrationDto
{

    public int? Ufid { get; set; } // UFID
	public int? Fdid { get; set; } // FDID
	public int? Hfid { get; set; } // HFID
    public int? OfficerCode { get; set; }
    public string? OfficerName { get; set; }
    public int? DistrictCode { get; set; }
    public string? DistrictNameEn { get; set; }
    public string? DistrictNameHi { get; set; }
    public int? SubDistrictCode { get; set; }
    public string? SubDistrictNameEn { get; set; }
    public string? SubDistrictNameHi { get; set; }
	public int? VillageCode { get; set; }
	public string? VillageName { get; set; }
	public string? FarmerNameEng { get; set; }
	public string? FarmerNameHi { get; set; }
	public string? FarmerFatherName { get; set; }
	public string? MobileNo { get; set; }
	public int? SchemeTypeId { get; set; } // scheme_type
	public int? SchemeId { get; set; } // schcme_id
	public string? SchemeName { get; set; } // scheme_name
	public string? ComponentName { get; set; } // cname
	public int? ComponentId { get; set; } // component_id
	public int? Unit { get; set; } // unit
	public decimal? Area { get; set; } // 
	public decimal? Cash { get; set; } // cash
	public decimal? Quantity { get; set; } // quantity
	public int? BenefitType { get; set; } // benefit_type
	public string? Benefit { get; set; } // benefit
	public string? BenefitTypeName { get; set; } // benefit_type_name
	public string? BenefitName { get; set; } // benefit_name
	public string? KhasraNo { get; set; } // khasra_no
	public string? TotalLandKhasra { get; set; } // total_land_khasra
	public decimal? TotalLandArea { get; set; } // total_land_area
}

