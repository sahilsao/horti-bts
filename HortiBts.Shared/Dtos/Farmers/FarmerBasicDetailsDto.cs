namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerBasicDetailsDto
    {
        public int? UfId { get; set; } // uf_id
        public string UniqueId { get; set; } // unique_id
        public string FarmerNameEng { get; set; } // farmer_name_eng
        public string FarmerNameHi { get; set; } // farmer_name_hi
        public string FatherName { get; set; } // father_name
        public string Relation { get; set; } // relation
        public DateTime? Dob { get; set; } // dob
        public string Category { get; set; } // category
        public string CasteName { get; set; } // caste_name
        public string Subcategory { get; set; } // subcategory
        public string SubcasteName { get; set; } // subcaste_name
        public string Gender { get; set; } // gender
        public string MobileNo { get; set; } // mobile_no
        public string MobileNo1 { get; set; } // mobile_no1
        public string DataSource { get; set; } // data_source
    }
}
