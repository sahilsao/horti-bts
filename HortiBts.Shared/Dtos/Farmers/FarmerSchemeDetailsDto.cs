namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerSchemeDetailsDto
    {
        public int? UfId { get; set; } // uf_id
        public int HfId { get; set; } // hf_id
        public int FdId { get; set; } // fd_id
        public int EquipmentId { get; set; } // equipment_id
        public int VillageCode { get; set; } // village_code
        public short SchemeType { get; set; } // scheme_type
        public string? SchemeName { get; set; } // scheme_name
        public short SchemeId { get; set; } // schcme_id
        public short ComponentId { get; set; } // component_id
        public string? Cname { get; set; } // cname
        public SByte BenefitType { get; set; } // benefit_type
        public string? BenefitTypeName { get; set; } // benefit_name_en
        public string? Benefit { get; set; } // benefit
        public string? BenefitName { get; set; } // benefit_name_hi
        public int Unit { get; set; } // unit
        public string? UnitName { get; set; } // unit_name
        public decimal Quantity { get; set; } // quantity
        public string? KhasraNo { get; set; } // khasra_no
        public decimal Area { get; set; } // area
        public decimal? Cash { get; set; } // cash
        public SByte FinancialYear { get; set; } // financial_year
        public string? VillageName { get; set; } // village_name
        public string? DistrictName { get; set; }
        public string? SubdistrictName { get; set; } // subdistrict_name
        public string? FinancialYearStr { get; set; } // financial_year
    }
}
