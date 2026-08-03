using System;
using System.Collections.Generic;
using System.Text;

namespace HortiBts.Shared.Dtos.Districts
{
    public class DistrictsDto
    {
        public int DistrictId { get; init; }
        public int DistrictCensus { get; init; }
        public string DistrictName { get; init; } = string.Empty;
        public string DistrictNameHindi { get; init; } = string.Empty;
        public int? DivId { get; init; }
        public int? CBankCode { get; init; }
    }
}
