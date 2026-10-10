namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationAddressDetailsDto
    {
        public int HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? FsId { get; set; } // fs_id
        public int? UfId { get; set; } // uf_id
        public string? Address { get; set; } // address
        public string? HouseNo { get; set; } // house_no
        public int? WardNo { get; set; } // ward_no
        public string? Pincode { get; set; } // pincode
        public string? MembershipNo { get; set; } // membership_no
        public int? FinancialYear { get; set; } // financial_year
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // 
        public int? SubdistrictCode { get; set; } // SubDistrictCodeCensus
        public string? SubdistrictName { get; set; } // BlockNameEng
        public int? DistrictCode { get; set; } // DistCodeCensus
        public string? DistrictName { get; set; }
    }
}
