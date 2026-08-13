namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerAddressDetailsDto
    {
        public int? UfId { get; set; } // uf_id
        public int HfId { get; set; } // hf_id
        public long FdId { get; set; } // fd_id
        public string HouseNo { get; set; } // house_no
        public int WardNo { get; set; } // ward_no
        public string Address { get; set; } // address
        public string Pincode { get; set; } // pincode
        public int VillageCode { get; set; } // village_code
        public string VillageName { get; set; } // village_name
        public int? DistCodeCensus { get; set; }
        public int? DistrictId { get; set; } // district_id
        public string DistrictName { get; set; }
        public int? SubdistrictCode { get; set; } // subdistrict_code
        public string SubdistrictName { get; set; } // subdistrict_name
        public int? FinancialYear { get; set; } // financial_year
        public string DataSource { get; set; } // data_source
    }
}
