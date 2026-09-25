namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Yearly
{
    public class BlockWiseFarmerRegistrationDto
    {
        public decimal? FarmerCount { get; set; } // farmer_count
        public decimal? SchemeCount { get; set; } // application_count
        public decimal? Area { get; set; } // area
        public decimal? Subsidy { get; set; } // subsidy
        public int? SubDistrictCode { get; set; } // district_code
        public string? SubDistrictNameHi { get; set; } // Block_name_hindi
        public string? SubDistrictNameEn { get; set; } // Block_name_english
        public int? DistrictCode { get; set; } // district_code
        public string? DistrictNameHi { get; set; } // district_name
        public string? DistrictNameEn { get; set; } // district_name
    }
}
