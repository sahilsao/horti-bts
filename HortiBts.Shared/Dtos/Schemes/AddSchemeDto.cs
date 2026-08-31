namespace HortiBts.Shared.Dtos.Schemes
{
    public class AddSchemeDto
    {
        public int? SchemeId { get; set; }
        public string? SchemeName { get; set; }      // Hindi display name
        public string? SchemeNameEn { get; set; }
        public string? SchemeCode { get; set; }
        public int? SchemeTypeId { get; set; }
        public string? SchemeTypeHi { get; set; }
        public string? SchemeTypeEn { get; set; }
        public string? SchemeDescriptionHi { get; set; }
        public string? SchemeDescriptionEn { get; set; }
    }
}
