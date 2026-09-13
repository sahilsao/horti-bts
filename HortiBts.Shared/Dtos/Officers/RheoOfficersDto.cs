namespace HortiBts.Shared.Dtos.Officers
{
    public class RheoOfficersDto
    {
        public int? OfficerCode { get; set; } // officer_code
        public string? OfficerName { get; set; } // name
        public string? MobileNo { get; set; } // mobile_no
        public string? AlternateMobileNo { get; set; } // 
        public string? Email { get; set; } // 
        public DateTime? ChargeTakenDate { get; set; } // charge_date
        public int? SubDistrictCode { get; set; } // subdistrict_code
        public string? SubDistrictName { get; set; } // subdistrict_name
        public int? DistrictCodeCensus { get; set; } // DistCodeCensus
        public string? DistrictName { get; set; }
    }
}