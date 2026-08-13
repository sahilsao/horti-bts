using System.Text.Json.Serialization;

namespace HortiBts.Shared.Models.Girdawari;

/// <summary>
/// One (village, khasra) pair to look up — mirrors the objects the Node.js
/// version expected inside the req.body array.
/// </summary>
public sealed class SearchParam
{
    [JsonPropertyName("village_code")]
    public int VillageCode { get; set; }

    [JsonPropertyName("khasra_no")]
    public string KhasraNo { get; set; } = string.Empty;

    [JsonPropertyName("financial_year")]
    public int FinancialYear { get; set; }
}

/// <summary>
/// One crop-detail record as returned by the SOAP service's embedded JSON.
/// Field names/casing below are inferred from the existing Angular table,
/// which binds directly to row.CropSeason, row.SubCropName, row.Khasra_No,
/// row.SubCropArea, row.VillageName, row.CropYear — i.e. these are the raw
/// SOAP JSON field names. Verify against a live response and adjust
/// [JsonPropertyName] values if any don't match exactly.
/// </summary>
public sealed class CropDetailRecord
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    /// <summary>1 = Rabi, 2 = Kharif, 3 = Jaid.</summary>
    [JsonPropertyName("CropSeason")]
    public int CropSeason { get; set; }

    [JsonPropertyName("SubCropName")]
    public string? SubCropName { get; set; }

    [JsonPropertyName("Khasra_No")]
    public string? KhasraNo { get; set; }

    [JsonPropertyName("SubCropArea")]
    public decimal SubCropArea { get; set; }

    [JsonPropertyName("VillageName")]
    public string? VillageName { get; set; }

    [JsonPropertyName("CropYear")]
    public string? CropYear { get; set; }

    // Catch anything not mapped above instead of silently dropping it.
    [JsonExtensionData]
    public Dictionary<string, object?>? ExtraFields { get; set; }
}
