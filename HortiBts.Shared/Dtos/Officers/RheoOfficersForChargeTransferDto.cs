namespace HortiBts.Shared.Dtos.Officers
{
    public class RheoOfficersForChargeTransferDto
    {
        public int? OfficerCode { get; set; }
        public string? OfficerName { get; set; }
        public string? MobileNo { get; set; }
        public string? AlternateMobileNo { get; set; } // alternate_mobile_no
        public string? Email { get; set; } // email
        public DateTime? ChargeDate { get; set; }
        public int IsVillageMapped { get; set; } // is_village_mapped
    }
}