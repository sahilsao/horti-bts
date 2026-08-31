using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Benefits
{
    public class BenefitsListDto
    {
        public long Srno { get; set; } //srno 
        public int BenefitId { get; set; } //benefit_id
        public int BenefitTypeId { get; set; }  //benefit_type_id
        public string? BenefitTypeNameEn { get; set; } //benefit_type_name_en
        public string? BenefitTypeNameHi { get; set; } //benefit_type_name_hi
        public string? BenefitNameEn { get; set; } //benefit_name_en
        public string? BenefitNameHi { get; set; } //benefit_name_hi
        public bool BenefitFlag { get; set; } //benefit_flag
    }
}
