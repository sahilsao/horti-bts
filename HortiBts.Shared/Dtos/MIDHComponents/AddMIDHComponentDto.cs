using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class AddMIDHComponentDto
    {
        public int? ComponentId { get; set; }
        public int? MidhComponentTypeId { get; set; }
        public int? MidhComponentId { get; set; }
        public string? MidhComponentNameEn { get; set; }
        public string? MidhComponentNameHi { get; set; }     
        public string? MidhComponentDescriptionEn { get; set; }
        public string? MidhComponentDescriptionHi { get; set; }
    }
}
