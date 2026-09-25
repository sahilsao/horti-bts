namespace HortiBts.Shared.Dtos.Reports.PreviousFY.BacklogYearly
{
    public class FarmerWiseFarmerRegistrationDto
    {
        public int? UFID { get; set; }
        public int? FDID { get; set; }
        public int? HFID { get; set; }
        public int? OfficerCode { get; set; }
        public string? OfficerName { get; set; }
        public int? DistrictCode { get; set; }
        public string? DistrictNameEn { get; set; }
        public string? DistrictNameHi { get; set; }
        public int? SubDistrictCode { get; set; }
        public string? SubDistrictNameEn { get; set; }
        public string? SubDistrictNameHi { get; set; }
        public int? VillageCode { get; set; }
        public string? VillageName { get; set; }
        public string? FarmerNameEng { get; set; }
        public string? FarmerNameHi { get; set; }
        public string? FarmerFatherName { get; set; }
        public string? MobileNo { get; set; }

        public int? TotalApplicationCountFyb4 { get; set; }
        public decimal? TotalAreaCoveredFyb4 { get; set; }
        public decimal? TotalCashDisbursedFyb4 { get; set; }

        public int? TotalApplicationCountFyb3 { get; set; }
        public decimal? TotalAreaCoveredFyb3 { get; set; }
        public decimal? TotalCashDisbursedFyb3 { get; set; }

        public int? TotalApplicationCountFyb2 { get; set; }
        public decimal? TotalAreaCoveredFyb2 { get; set; }
        public decimal? TotalCashDisbursedFyb2 { get; set; }

        public int? TotalApplicationCountFyb1 { get; set; }
        public decimal? TotalAreaCoveredFyb1 { get; set; }
        public decimal? TotalCashDisbursedFyb1 { get; set; }

        public string? TotalLandKhasra { get; set; }
        public decimal? TotalLandArea { get; set; }
    }
}
