namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerLandDetailsHPMISDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? IsLandEntered { get; set; } // is_land_entered
        public int? LdId { get; set; } // ld_id
        public int? Fid { get; set; } // f_id
        public int? UfId { get; set; } // uf_id
        public int? UserId { get; set; } // user_id
        public string? MobileNo { get; set; } // mobile_no
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // villcdname
        public int? DistrictCode { get; set; } // district_code
        public int? SubdistrictCode { get; set; } // block_code
        public string? OwnerName { get; set; } // owner_name
        public string? FatherName { get; set; } // father_name
        public long? PatwariHalka { get; set; } // patwari_halka
        public string? KhasraNo { get; set; } // khasra_no
        public string? BasraNo { get; set; } // basra_no
        public decimal? LandArea { get; set; } // land_area
        public string? OwnerType { get; set; } // owner_type
        public string? OwnerTypeCode { get; set; } // owner_type_code
        public string? VillageType { get; set; } // village_type
        public int? FinancialYear { get; set; } // financial_year
        public int? JointKhasraId { get; set; } // joint_khasra_id
        public string? JointKhasraDetails { get; set; } // joint_khasra_details
        public string? DistrictNameHi { get; set; } // district_name_hi
        public string? SubdistrictNameHi { get; set; } // block_name_hi
    }
}
