namespace HortiBts.Shared.Dtos.MIDHSchemes
{
    public class AddMIDHSchemeDto
    {
        public int? SchemeId { get; set; }
        public int? SchemeTypeId { get; set; }
        public int? MIDHSchemeId { get; set; }
        public string? MIDHSchemeNameHi { get; set; }      // Hindi display name
        public string? MIDHSchemeNameEn { get; set; }
        public string? MIDHSchemeCode { get; set; }
        public string? MIDHSchemeTypeHi { get; set; }
        public string? MIDHSchemeTypeEn { get; set; }
        public string? MIDHSchemeDescriptionHi { get; set; }
        public string? MIDHSchemeDescriptionEn { get; set; }
    }
}
