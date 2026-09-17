using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Reports.Yearly
{
    public class RheoWiseFarmerRegistrationDto
    {
        public int? FarmerCount { get; set; }
        public int? SchemeCount { get; set; }
        public decimal? Area { get; set; }
        public decimal? Subsidy { get; set; }

        public int? OfficerCode { get; set; }
        public string? OfficerName { get; set; }
        public string? MobileNo { get; set; } 

        public int? SubDistrictCode { get; set; }
        public string? SubDistrictNameHi { get; set; }
        public string? SubDistrictNameEn { get; set; } 

        public int? DistrictCode { get; set; }
        public string? DistrictNameHi { get; set; } 
        public string? DistrictNameEn { get; set; } 
    }
}
