namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerBasicDetailsHPMISDto
    {
        public int? Fid { get; set; } // f_id
        public int? ApplicationId { get; set; } // application_id
        public DateTime? ApplicationDate { get; set; } // created_at
        public int? UfId { get; set; } // uf_id
        public int? UserId { get; set; } // user_id
        public string? MobileNo { get; set; } // mobile_no
        public int? AadharNumber { get; set; } // aadhar_number
        public string? FarmerNameEng { get; set; } // farmer_name_eng
        public string? FarmerNameHi { get; set; } // farmer_name_hi
        public string? CareOfName { get; set; } // care_of_name
        public DateTime? Dob { get; set; } // dob
        public int? Relation { get; set; } // relation
        public string? RelName { get; set; } // rel_name
        public int? Category { get; set; } // category
        public string? CasteName { get; set; } // caste_name
        public short? Subcategory { get; set; } // subcategory
        public string? SubcasteName { get; set; } // subcaste_name
        public string? Gender { get; set; } // gender
        public string? GenderNameHi { get; set; } // gender_name_hi
        public string? MobileNo1 { get; set; } // mobile_no1
        public int? DistrictCode { get; set; } // district_code
        public string? DistrictNameHi { get; set; } // district_name_hi
        public int? SubdistrictCode { get; set; } // subdistrict_code
        public string? SubdistrictName { get; set; } // block_name_eng
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // villcdname
        public string? HouseNo { get; set; } // house_no
        public string? WardNo { get; set; } // ward_no
        public string? Address { get; set; } // address
        public string? FinalAddress { get; set; } // 
        public int? PinCode { get; set; } // pin_code
        public int? WorkingMember { get; set; } // working_member
        public decimal? FamilyIncome { get; set; } // family_income
        public string? DataSource { get; set; } // data_source
    }
}
