namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationSchemeDetailsDto
    {
        public int? SdId { get; set; } // sd_id
        public int? FdId { get; set; } // fd_id
        public int? UfId { get; set; } // uf_id
        public int? HfId { get; set; } // hf_id
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // village_name
        public int? SchemeType { get; set; } // scheme_type
        public string? SchemeTypeName { get; set; } // scheme_name
        public string? SchemeName { get; set; } // scheme_name
        public string? Cname { get; set; } // cname
        public int? Quantity { get; set; } // quantity
        public string? KhasraNo { get; set; } // khasra_no
        public decimal? Area { get; set; } // area
        public int? FinancialYear { get; set; } // financial_year
        public string? Status { get; set; } // status
    }
}
