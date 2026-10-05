namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerMarketLinkageDetailsHPMISDto
    {
        public int? Mid { get; set; } // m_id
        public int? Fid { get; set; } // f_id
        public int? UfId { get; set; } // uf_id
        public int? UserId { get; set; } // user_id
        public string? MobileNo { get; set; } // mobile_no
        public int? MarketLinkType { get; set; } // market_link_type
        public string? MarketNameHi { get; set; } // market_name_hi
        public string? BuyerName { get; set; } // buyer_name
        public int? FinancialYear { get; set; } // financial_year
        public int? MandiCode { get; set; } // mandi_code
        public string? MandiNameHi { get; set; } // mandi_name_hi
        public string? CropNameHi { get; set; } // crop_name_hi
        public string? CategoryNameHi { get; set; } // category_name_hi
        public string? VarietyName { get; set; } // variety_name
        public string? Address { get; set; } // 
        public int? DistrictCode { get; set; } // district_code
        public int? TehsilCode { get; set; } // tehsil_code
        public int? StateCode { get; set; } // state_code
        public int? CountryCode { get; set; } // country_code
    }
}
