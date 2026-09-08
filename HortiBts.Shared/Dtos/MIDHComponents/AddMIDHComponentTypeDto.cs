using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class AddMIDHComponentTypeDto
    {
        public int? ComponentTypeId { get; set; }
        public int? MidhSchemeId { get; set; }
        public int? MidhComponentTypeId { get; set; }
        public string? MidhComponentTypeNameEn { get; set; }
        public string? MidhComponentTypeNameHi { get; set; }     
        public string? MidhComponentTypeDescriptionEn { get; set; }
        public string? MidhComponentTypeDescriptionHi { get; set; }
    }
}
