using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Benefits
{
    public class AddBenefitDto
    {
        public int? BenefitId { get; set; } // benefit_id
        public int BenefitTypeId { get; set; } // benefit_type_id
        public string? BenefitNameHi { get; set; } // benefit_name_hi
        public string? BenefitNameEn { get; set; } // benefit_name_en
        public bool Flag { get; set; } // flag
    }
}
