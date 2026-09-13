using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Reports
{
    public class DistWiseFarmerRegistrationDto
    {
        public int? Target { get; set; } // total_target
        public decimal? FarmerCount { get; set; } // farmer_count
        public decimal? SchemeCount { get; set; } // application_count
        public decimal? Area { get; set; } // area
        public decimal? Subsidy { get; set; } // subsidy
        public int? DistrictCode { get; set; } // district_code
        public string? DistrictNameHi { get; set; } // district_name_hindi
        public string? DistrictNameEn { get; set; } // district_name_english
    }
}
