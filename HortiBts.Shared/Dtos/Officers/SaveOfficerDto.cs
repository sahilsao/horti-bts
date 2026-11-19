using System;

namespace HortiBts.Shared.Dtos.Officers;

public class SaveOfficerDto
{
        public int Role { get; set; }                 // designation code -> mas_raeo.role / usertype and village department_code
        public string Name { get; set; } = "";
        public string CircleName { get; set; } = "";
        public string MobileNo { get; set; } = "";
        public string? AlternateMobileNo { get; set; }
        public string? Email { get; set; }
        public int VillageCount { get; set; }         // totalcount
        public int SubDistrictCode { get; set; }
        public int DistrictCode { get; set; }
        public DateTime? ChargeDate { get; set; }
        public List<int> VillageCodes { get; set; } = new();
        // created_by and ip_address are set on the server

}
