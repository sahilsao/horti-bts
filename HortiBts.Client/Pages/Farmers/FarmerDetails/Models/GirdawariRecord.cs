using System.Text.Json.Serialization;

namespace HortiBts.Client.Pages.Farmers.FarmerDetails.Models
{
    /// <summary>Mirrors GirdawariApi.Models.SearchParam sent to the WebAPI.</summary>
    public sealed class SearchParam
    {
        [JsonPropertyName("village_code")]
        public int VillageCode { get; set; }

        [JsonPropertyName("khasra_no")]
        public string KhasraNo { get; set; } = string.Empty;

        [JsonPropertyName("financial_year")]
        public int FinancialYear { get; set; }
    }

    /// <summary>Mirrors GirdawariApi.Models.CropDetailRecord returned by the WebAPI.</summary>
    public sealed class GirdawariRecord
    {
        public int Status { get; set; }

        /// <summary>1 = Rabi, 2 = Kharif, 3 = Jaid.</summary>
        public int CropSeason { get; set; }

        public string? SubCropName { get; set; }

        [JsonPropertyName("Khasra_No")]
        public string? KhasraNo { get; set; }

        public decimal SubCropArea { get; set; }
        public string? VillageName { get; set; }
        public string? CropYear { get; set; }

        public string CropSeasonLabel => CropSeason switch
        {
            1 => "Rabi",
            2 => "Kharif",
            3 => "Jaid",
            _ => string.Empty
        };
    }

    /// <summary>
    /// One financial-year tab's worth of girdawari data — equivalent to a single
    /// entry pushed into the Angular component's girdawariDataSource array.
    /// </summary>
    public sealed class GirdawariYearTab
    {
        public required int YearId { get; init; }
        public required string YearText { get; init; }
        public required List<GirdawariRecord> Items { get; init; }
    }

}
