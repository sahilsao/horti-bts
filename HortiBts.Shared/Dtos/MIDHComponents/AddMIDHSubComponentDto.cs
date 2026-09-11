namespace HortiBts.Shared.Dtos.MIDHComponents
{
    public class AddMIDHSubComponentDto
    {
        public int? SubComponentId { get; set; }
        public int? MidhSubComponentId { get; set; }
        public int? MidhComponentId { get; set; } //for match with mas_component_new table (autoincrementid)
        public string? MidhSubComponentNameEn { get; set; }
        public string? MidhSubComponentNameHi { get; set; }
        public string? MidhSubComponentDescriptionEn { get; set; }
        public string? MidhSubComponentDescriptionHi { get; set; }
    }
}
