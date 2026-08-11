using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.FinancialYear
{
    public record FinancialYearDto
    {
        public int Id { get; set; }
        public string FinancialYear { get; set; } = string.Empty;
    }
}
