using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Units
{
    public class UnitDto
    {
        public int UnitId { get; set; } // unit_id
        public string? UnitName { get; set; } // unit_name
        public SByte BenefitTypeId { get; set; } // benefit_type_id
        public DateTime? UploadDate { get; set; } // upload_date
        public string? Flag { get; set; } // flag
    }
}
