namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationBasicDetailsDto
    {
        public string? MembershipNo { get; set; }
        public string? AadharNumber { get; set; } // aadhar_number
        public int? HfId { get; set; } // hf_id
        public int? UfId { get; set; } // uf_id
        public string? PoFarmercode { get; set; } // po_farmercode
        public string? FarmerNameEng { get; set; } // farmer_name_eng
        public string? FarmerNameHi { get; set; } // farmer_name_hi
        public string? Relation { get; set; } // relation
        public string? FatherName { get; set; } // father_name
        public DateTime? Dob { get; set; } // dob
        public string? Category { get; set; } // category
        public string? Subcategory { get; set; } // subcategory
        public string? Gender { get; set; } // gender
        public string? MobileNo { get; set; } // mobile_no
        public string? DataSource { get; set; } // data_source
    }
}
