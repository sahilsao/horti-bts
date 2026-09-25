namespace HortiBts.Shared.Dtos.Reports.PreviousFY.Comparative
{
    public class RheoWiseComparativeFarmerRegistrationDto
    {
        public int? TotalFarmerCountFyb4 { get; set; } // 
        public int? TotalFarmerCountFyb3 { get; set; } // 
        public int? TotalFarmerCountFyb2 { get; set; } // 
        public int? TotalFarmerCountFyb1 { get; set; } // 
        public int? OfficerCode { get; set; }
        public string? OfficerName { get; set; }
        public string? MobileNo { get; set; }
        public int? DistrictCode { get; set; }
        public string? DistrictNameHi { get; set; }
        public string? DistrictNameEn { get; set; }
        public int? SubDistrictCode { get; set; }
        public string? SubDistrictNameHi { get; set; }
        public string? SubDistrictNameEn { get; set; }
    }
}
