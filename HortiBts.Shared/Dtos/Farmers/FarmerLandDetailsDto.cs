namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerLandDetailsDto
    {
        public int UfId { get; set; } // uf_id
        public int HfId { get; set; } // hf_id
        public long FdId { get; set; } // fd_id
        public int LdId { get; set; } // ld_id
        public int VillageCode { get; set; } // village_code
        public string? OwnerName { get; set; }
        public string? Ownertype { get; set; } // ownertype
        public string? FatherName { get; set; }
        public long? PatwariHalka { get; set; } // patwari_halka
        public string? KhasraNo { get; set; } // khasra_no
        public decimal? Area { get; set; } // area
        public string? Type { get; set; } // type
        public string? VillageName { get; set; } // village_name
        public int? DistCodeCensus { get; set; }
        public int? DistrictId { get; set; } // district_id
        public string? DistrictName { get; set; }
        public int? SubDistrictCode { get; set; } // subdistrict_code
        public string? SubDistrictName { get; set; } // subdistrict_name
        public int FinancialYear { get; set; } // financial_year
        public string? FinancialYearStr { get; set; } // financial_year
    }
}
