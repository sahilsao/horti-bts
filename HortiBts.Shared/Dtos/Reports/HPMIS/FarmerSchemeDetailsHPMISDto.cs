namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerSchemeDetailsHPMISDto
    {
        public int? Sid { get; set; } // s_id
        public int? Fid { get; set; } // f_id
        public int? SchemeId { get; set; } // scheme_id
        public int? SchemeTypeId { get; set; } // scheme_type_id
        public int? ComponentId { get; set; } // component_id
        public int? FinancialYear { get; set; } // financial_year
        public string? SchemeName { get; set; } // scheme_name
        public string? SchemeTypeName { get; set; } // scheme_name
        public string? ComponentName { get; set; } // cname
    }
}
