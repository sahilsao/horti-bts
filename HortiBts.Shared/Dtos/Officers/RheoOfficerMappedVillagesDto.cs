namespace HortiBts.Shared.Dtos.Officers
{
    public class RheoOfficerMappedVillagesDto
    {
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; }
        public string? Halka { get; set; }
        public int? SubDistrictCode { get; set; } // subdistrict_code
        public string? SubDistrictName { get; set; } // BlockNameEng
        public int? DistrictCode { get; set; } // DistCodeCensus
        public int? DistrictId { get; set; } // district_id
        public string? DistrictName { get; set; }
        public DateTime? LastAssignDate { get; set; }
    }
}