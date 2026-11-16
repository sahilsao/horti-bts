using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Villages
{
    public class VillagesForRHEODto
    {
        public int? VillageCode { get; set; } // village_code
        public int? VillageLgdCode { get; set; } // village_lgd_code
        public string? VillageName { get; set; } // village_name
        public int? SubdistrictCode { get; set; } // subdistrict_code
        public string? SubdistrictName { get; set; } // BlockNameEng
    }
}
