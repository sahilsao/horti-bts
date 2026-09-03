using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Target
{
    public class TargetDto
    {
        public int TargetId { get; set; } // id
        public int UserId { get; set; } // user_id
        public string? UserNameEn { get; set; } // DistrictNameEnglish
        public string? UserNameHi { get; set; } // DistrictNameHindi
        public int UserType { get; set; } // user_type
        public string? UserTypeName { get; set; } // user_type_name
        public int TargetCount { get; set; } // target_count
        public decimal TargetArea { get; set; } // target_area
        public int FinancialYearId { get; set; } // financial_year_id
        public string? FinancialYear { get; set; } // financial_year
    }
}
