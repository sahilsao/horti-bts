using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Components
{
    public class ComponentDto
    {
        public int ComponentId { get; set; } // c_id
        public string? ComponentName { get; set; } // cname
        public string? ComponentNameHi { get; set; } // cname_hi
        public string? ComponentDescriptionEn { get; set; } // description_en
        public string? ComponentDescriptionHi { get; set; } // description_hi
        public int? ComponentUnitId { get; set; } // unit_id
        public int? SchemeId { get; set; } // s_id
        public string? SchemeName { get; set; } // scheme_name
        public string? SchemeNameEn { get; set; } // scheme_name_en
        public int? SchemeTypeId { get; set; } // st_id
    }
}
