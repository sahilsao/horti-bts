using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Benefits
{
    public class BenefitsTypeDto
    {
        public int BenefitTypeId { get; set; } // benefit_type_id
        public string? BenefitNameEn { get; set; } // benefit_name_en
        public string? BenefitNameHi { get; set; } // benefit_name_hi
    }
}
