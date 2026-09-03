using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Gallery
{
    [ApiController]
    [Route("api/gallery")]
    public class GalleryController(
        IWebHostEnvironment env,
        IConfiguration config,
        ILogger<GalleryController> logger) : ControllerBase
    {
        private static readonly string[] AllowedExtensions =
        [
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif"
        ];

        [HttpGet]
        [EndpointSummary("Get scheme gallery images")]
        [EndpointDescription("Retrieves the list of available scheme gallery images along with their file names and URLs.")]
        public ActionResult<IEnumerable<SchemesGalleryDto>> GetImages()
        {
            try
            {
                var baseUrl =
                    config["ApiBaseUrl"]?.TrimEnd('/')
                    ?? $"{Request.Scheme}://{Request.Host}";

                var folder = Path.Combine(
                    env.WebRootPath,
                    "images",
                    "schemes");

                if (!Directory.Exists(folder))
                    return Ok(Enumerable.Empty<SchemesGalleryDto>());

                var images = Directory
                    .EnumerateFiles(folder)
                    .Where(f => AllowedExtensions.Contains(
                        Path.GetExtension(f),
                        StringComparer.OrdinalIgnoreCase))
                    .OrderBy(Path.GetFileName)
                    .Select(file => new SchemesGalleryDto
                    {
                        FileName = Path.GetFileName(file),
                        Url = $"{baseUrl}/images/schemes/{Path.GetFileName(file)}"
                    });

                return Ok(images);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load gallery images.");

                return Problem(
                    "Failed to load gallery images.",
                    statusCode: 500);
            }
        }
    }
}