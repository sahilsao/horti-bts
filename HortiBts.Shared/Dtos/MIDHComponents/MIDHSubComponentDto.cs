using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class MIDHSubComponentDto
    {
        public int SubComponentId { get; set; } // sub_component_id
        public int ComponentId { get; set; } // component_id from mas_component_new table to match with mas_sub_component_new table
        public string? MidhSubComponentNameEn { get; set; } // sub_component_name_en
        public string? MidhSubComponentNameHi { get; set; } // sub_component_name_hi
        public string? MidhSubComponentDescriptionEn { get; set; } // description_en
        public string? MidhSubComponentDescriptionHi { get; set; } // description_hi
        public int MidhSubComponentId { get; set; } // midh_sub_component_id
        public string? MidhComponentNameEn { get; set; } // component_name_en
        public string? MidhComponentNameHi { get; set; } // component_name_hi
        public int MidhComponentTypeId { get; set; } // component_type_id
        public string? MidhComponentTypeNameEn { get; set; } // component_type_name_en
        public string? MidhComponentTypeNameHi { get; set; } // component_type_name_hi
        public int MidhSchemeTypeId { get; set; } // scheme_type_id
        public int MidhSchemeId { get; set; } // scheme_id
        public string? MidhSchemeNameEn { get; set; } // scheme_name_en
        public string? MidhSchemeNameHi { get; set; } // scheme_name_hi
        public bool Flag { get; set; } // flag
    }
}
