namespace HortiBts.Shared.Dtos.MIDHSchemes;

// Maps to mas_scheme_file_path_new.
// GET /api/midhschemes/{schemeId}/documents
public class MIDHSchemeDocDto
{
    public int? Id { get; set; }
    public int? SchemeId { get; set; }
    public string FileName { get; set; } = "";   // human-readable label, for display
    public string Path { get; set; } = "";       // actual filename on disk, used for download
}

// Maps to mas_scheme_horti (self-joined for its own "type" row via st_id).
// GET /api/midhschemes/{stId} -- stId 2 = centrally sponsored, 1 = state sponsored (per existing contract).
public class MIDHSchemeDto
{
    public int? SchemeId { get; set; } // scheme_id
    public int? SchemeTypeId { get; set; } // scheme_type_id
    public int? MidhSchemeId { get; set; } // midh_scheme_id
    public string? MidhSchemeTypeNameHi { get; set; } // scheme_type_name_hi
    public string? MidhSchemeTypeNameEn { get; set; } // scheme_type_name_en
    public string? MidhSchemeNameEn { get; set; } // scheme_name_en
    public string? MidhSchemeNameHi { get; set; } // scheme_name_hi
    public string? MidhSchemeDescriptionEn { get; set; } // description_en
    public string? MidhSchemeDescriptionHi { get; set; } // description_hi
    public string? Flag { get; set; }
    public string? FilePath { get; set; }

}

// GET /api/manuals
public record Manual(string TitleEn, string TitleHi, string File);

public class MIDHSchemesGalleryDto
{
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}