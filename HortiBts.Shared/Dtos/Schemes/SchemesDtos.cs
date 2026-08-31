namespace HortiBts.Shared.Dtos.Schemes;

// Maps to mas_scheme_file_path.
// GET /api/schemes/{schemeId}/documents
public class SchemeDocDto
{
    public int? Id { get; set; }
    public int? SchemeId { get; set; }
    public string FileName { get; set; } = "";   // human-readable label, for display
    public string Path { get; set; } = "";       // actual filename on disk, used for download
}

// Maps to mas_scheme_horti (self-joined for its own "type" row via st_id).
// GET /api/schemes/{stId} -- stId 2 = centrally sponsored, 1 = state sponsored (per existing contract).
public class SchemeDto
{
    public int Srno { get; set; }
    public int? SchemeId { get; set; }
    public string? SchemeName { get; set; }       // Hindi display name
    public string? SchemeNameEn { get; set; }
    public string? SchemeCode { get; set; }
    public int? SchemeTypeId { get; set; }
    public string? SchemeTypeHi { get; set; }
    public string? SchemeTypeEn { get; set; }
    public string? SchemeDescriptionHi { get; set; }
    public string? SchemeDescriptionEn { get; set; }
    public string? IsBeneficiary { get; set; }          // "Y" / "N"
    public string? Flag { get; set; }
    public string? FilePath { get; set; }

    public bool IsBeneficiaryFlag => IsBeneficiary == "Y";
}

// GET /api/manuals
public record Manual(string TitleEn, string TitleHi, string File);

public class SchemesGalleryDto
{
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}