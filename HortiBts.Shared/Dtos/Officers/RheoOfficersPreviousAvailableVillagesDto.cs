namespace HortiBts.Shared.Dtos.Officers
{
    public class RheoOfficersPreviousAvailableVillagesDto
    {
        public long Srno { get; set; } // 
        public int VillageCode { get; set; }
        public string? VillageName { get; set; }
        public string? Halka { get; set; }
        public int? SubdistrictCode { get; set; }
        public string? SubdistrictName { get; set; }
        public int? DistrictCode { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public DateTime? LastAssignDate { get; set; } // last_assign_date
    }
}