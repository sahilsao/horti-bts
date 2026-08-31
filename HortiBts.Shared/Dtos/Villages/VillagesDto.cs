using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Villages
{
    public class VillagesDto
    {
        public int VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // village_name
        public string? VillageNameHi { get; set; } // village_name_hi
        public int? TehsilCensus { get; set; } //tehsil_census 
        public int? SubdistrictCode { get; set; } // SubDistrictCodeCensus
    }
}
