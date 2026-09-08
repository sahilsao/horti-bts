using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class MIDHComponentTypeDto
    {
        public int ComponentTypeId { get; set; } // component_type_id
        public int MidhSchemeId { get; set; } // scheme_id
        public string? MidhSchemeNameEn { get; set; } // scheme_name_en
        public string? MidhSchemeNameHi { get; set; } // scheme_name_hi
        public string? MidhComponentTypeNameEn { get; set; } // component_type_name_en
        public string? MidhComponentTypeNameHi { get; set; } // component_type_name_hi
        public int SchemeTypeId { get; set; } // scheme_type_id
        public string? MidhComponentDescriptionEn { get; set; } // description_en
        public string? MidhComponentDescriptionHi { get; set; } // description_hi
        public int MidhComponentTypeId { get; set; } // midh_component_type_id
        public bool Flag { get; set; } // flag
    }
}
