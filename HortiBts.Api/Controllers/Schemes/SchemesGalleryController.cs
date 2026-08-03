using HortiBts.Shared.Dtos.Schemes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HortiBts.Api.Controllers.Schemes
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemesGalleryController(IWebHostEnvironment env, ILogger<SchemesGalleryController> logger) : ControllerBase
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
        public ActionResult<IEnumerable<SchemesGalleryDto>> GetImages()
        {
            try
            {
                var folder = Path.Combine(env.WebRootPath, "images", "schemes");

                if (!Directory.Exists(folder))
                    return Ok(Enumerable.Empty<SchemesGalleryDto>());

                var images = Directory
                    .EnumerateFiles(folder)
                    .Where(f => AllowedExtensions.Contains(Path.GetExtension(f), StringComparer.OrdinalIgnoreCase))
                    .OrderBy(Path.GetFileName)
                    .Select(file => new SchemesGalleryDto
                    {
                        FileName = Path.GetFileName(file),
                        Url = $"images/schemes/{Path.GetFileName(file)}"
                    });

                return Ok(images);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load gallery images.");
                return Problem("Failed to load gallery images.", statusCode: 500);
            }
        }
    }
}
