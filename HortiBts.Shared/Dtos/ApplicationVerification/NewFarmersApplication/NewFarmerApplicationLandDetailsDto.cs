namespace HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication
{
    public record NewFarmerApplicationLandDetailsDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? UfId { get; set; } // uf_id
        public int? HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? LdId { get; set; } // ld_id
        public int? VillageCode { get; set; } // village_code
        public string? OwnerName { get; set; }
        public string? Ownertype { get; set; } // ownertype
        public string? FatherName { get; set; }
        public int? PatwariHalka { get; set; } // patwari_halka
        public string? KhasraNo { get; set; } // khasra_no
        public decimal? Area { get; set; } // area
        public string? VillageName { get; set; } // village_name
        public string? IsJointKhasra { get; set; } // 
        public int? DistCodeCensus { get; set; }
        public int? DistrictId { get; set; } // district_id
        public string? DistrictName { get; set; }
        public int? SubdistrictCode { get; set; } // subdistrict_code
        public string? SubdistrictName { get; set; } // subdistrict_name
        public int? FinancialYear { get; set; } // financial_year
        public string? FinancialYearStr { get; set; } // financial_year
    }
}
