using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Components
{
    public class AddComponentDto
    {
        public int? SchemeId { get; set; }
        public int? ComponentId { get; set; }
        public int? ComponentUnitId { get; set; }
        public string? ComponentNameHi { get; set; }     
        public string? ComponentNameEn { get; set; }
        public string? ComponentDescriptionHi { get; set; }
        public string? ComponentDescriptionEn { get; set; }
    }
}
