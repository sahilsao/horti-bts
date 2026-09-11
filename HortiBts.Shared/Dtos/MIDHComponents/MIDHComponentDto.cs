using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class MIDHComponentDto
    {
        public int ComponentId { get; set; } // component_id
        public int MidhComponentTypeId { get; set; } // component_type_id
        public int MidhComponentId { get; set; } // midh_component_id
        public string? MidhComponentNameEn { get; set; } // component_name_en
        public string? MidhComponentNameHi { get; set; } // component_name_hi
        public string? MidhComponentDescriptionEn { get; set; } // description_en
        public string? MidhComponentDescriptionHi { get; set; } // description_hi
        public string? MidhComponentTypeNameEn { get; set; } // component_type_name_en
        public string? MidhComponentTypeNameHi { get; set; } // component_type_name_hi
        public int MidhSchemeId { get; set; } // scheme_id
        public int MidhSchemeTypeId { get; set; } // scheme_type_id
        public string? MidhSchemeNameEn { get; set; } // scheme_name_en
        public string? MidhSchemeNameHi { get; set; } // scheme_name_hi
        public bool Flag { get; set; } // flag
    }
}
