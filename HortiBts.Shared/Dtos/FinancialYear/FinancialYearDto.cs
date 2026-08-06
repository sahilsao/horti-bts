using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.FinancialYear
{
    public record FinancialYearDto
    {
        public int Id { get; init; }
        public string FinancialYear { get; init; } = string.Empty;
    }
}
