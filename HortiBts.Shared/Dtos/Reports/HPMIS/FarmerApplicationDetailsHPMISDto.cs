namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerApplicationDetailsHPMISDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? Fid { get; set; } // f_id
        public string? MobileNumber { get; set; } // mobile_number
        public int? DistrictCode { get; set; } // district_code
        public int? SubdistrictCode { get; set; } // subdistrict_code
        public int? VillageCode { get; set; } // village_code
        public int? FinancialYear { get; set; } // financial_year
        public int? IsLandEntered { get; set; } // is_land_entered
        public int? IsCropEntered { get; set; } // is_crop_entered
        public int? IsMarketLinkageEntered { get; set; } // is_market_linkage_entered
        public int? IsSchemeEntered { get; set; } // is_scheme_entered
        public DateTime? CreatedAt { get; set; } // created_at
        public string? IsApproved { get; set; } // is_approved
        public string? Remark { get; set; } // remark
    }
}
