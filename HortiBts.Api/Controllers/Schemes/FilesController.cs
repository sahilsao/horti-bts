using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace HortiBts.Api.Controllers.SchemesManuals;

// Serves files straight from wwwroot/docs/{scheme_files|notification_files|manual_files}, forcing a
// download (Content-Disposition: attachment)
[ApiController]
[Route("api/files")]
public class FilesController(IWebHostEnvironment env, ILogger<FilesController> logger) : ControllerBase
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    // GET api/files/scheme/{fileName}
    // Files at: wwwroot/docs/scheme_files/{fileName}
    [HttpGet("scheme/{fileName}")]
    public IActionResult GetSchemeFile(string fileName) => ServeFile("scheme_files", fileName);

    // GET api/files/notification/{fileName}
    // Files at: wwwroot/docs/notification_files/{fileName}
    [HttpGet("notification/{fileName}")]
    public IActionResult GetNotificationFile(string fileName) => ServeFile("notification_files", fileName);

    // GET api/files/manual/{fileName}
    // Files at: wwwroot/docs/manual_files/{fileName}
    [HttpGet("manual/{fileName}")]
    public IActionResult GetManualFile(string fileName) => ServeFile("manual_files", fileName);

    private IActionResult ServeFile(string subfolder, string fileName)
    {
        // Guard against path traversal (e.g. "../../appsettings.json") since fileName comes
        // straight from the URL/DB.
        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName) || safeFileName != fileName)
        {
            return BadRequest("Invalid file name.");
        }

        var fullPath = Path.Combine(env.WebRootPath, "docs", subfolder, safeFileName);

        if (!System.IO.File.Exists(fullPath))
        {
            logger.LogWarning("File not found: {FullPath}", fullPath);
            return NotFound();
        }

        if (!ContentTypeProvider.TryGetContentType(fullPath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        var bytes = System.IO.File.ReadAllBytes(fullPath);
        return File(bytes, contentType, safeFileName); // fileDownloadName forces Content-Disposition: attachment
    }
}