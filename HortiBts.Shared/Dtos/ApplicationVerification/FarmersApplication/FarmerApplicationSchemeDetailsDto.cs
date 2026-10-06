namespace HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication
{
    public record FarmerApplicationSchemeDetailsDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? UfId { get; set; } // uf_id
        public int? HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? SdId { get; set; } // sd_id
        public int? VillageCode { get; set; } // village_code
        public int? SchemeType { get; set; } // scheme_type
        public string? SchemeTypeName { get; set; } // scheme_name
        public string? SchemeName { get; set; } // scheme_name
        public int? SchemeId { get; set; } // scheme_id
        public int? ComponentId { get; set; } // component_id
        public string? Cname { get; set; } // cname
        public string? Benefit { get; set; } // benefit
        public int? Quantity { get; set; } // quantity
        public string? KhasraNo { get; set; } // khasra_no
        public decimal? Area { get; set; } // area
        public int? FinancialYear { get; set; } // financial_year
        public string? VillageName { get; set; } // village_name
        public string? DistrictName { get; set; }
        public string? SubdistrictName { get; set; } // subdistrict_name
        public string? FinancialYearStr { get; set; } // financial_year
    }
}
