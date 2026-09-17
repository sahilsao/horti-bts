namespace HortiBts.Shared.Dtos.Reports.BacklogYearly
{
    public class VillageWiseFarmerRegistrationDto
    {
        public int? TotalFarmerCountFyb4 { get; set; } // 
        public int? TotalApplicationCountFyb4 { get; set; } // 
        public decimal? TotalAreaCoveredFyb4 { get; set; } // 
        public decimal? TotalCashDisbursedFyb4 { get; set; } // 
        public int? TotalFarmerCountFyb3 { get; set; } // 
        public int? TotalApplicationCountFyb3 { get; set; } // 
        public decimal? TotalAreaCoveredFyb3 { get; set; } // 
        public decimal? TotalCashDisbursedFyb3 { get; set; } // 
        public int? TotalFarmerCountFyb2 { get; set; } // 
        public int? TotalApplicationCountFyb2 { get; set; } // 
        public decimal? TotalAreaCoveredFyb2 { get; set; } // 
        public decimal? TotalCashDisbursedFyb2 { get; set; } // 
        public int? TotalFarmerCountFyb1 { get; set; } // 
        public int? TotalApplicationCountFyb1 { get; set; } // 
        public decimal? TotalAreaCoveredFyb1 { get; set; } // 
        public decimal? TotalCashDisbursedFyb1 { get; set; } // 
        public int VillageCode { get; set; }
        public string? VillageName { get; set; }
        public int? DistrictCode { get; set; }
        public string? DistrictNameEn { get; set; }
        public string? DistrictNameHi { get; set; }
        public int? SubDistrictCode { get; set; }
        public string? SubDistrictNameEn { get; set; }
        public string? SubDistrictNameHi { get; set; }
        public int? OfficerCode { get; set; }
        public string? OfficerName { get; set; } // name
        public string? MobileNo { get; set; } // mobile_no
    }
}
