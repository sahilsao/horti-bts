using System;

namespace HortiBts.Shared.Dtos.Reports;

public class VillageWiseFarmerRegistrationDto
{

        public int? FarmerCount { get; set; } // f_count
        public int? SchemeCount { get; set; } // s_count
        public decimal? Area { get; set; } // area
        public decimal? Subsidy { get; set; } // subsidy
        public int? DistrictCode { get; set; } // DistCodeCensus
        public int? SubDistrictCode { get; set; } // subdistrict_code
        public int VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // village_name
        public int? OfficerCode { get; set; } // officer_code
        public string? OfficerName { get; set; } // officer_name
        public string? MobileNo { get; set; } // officer_mobile_no
}
