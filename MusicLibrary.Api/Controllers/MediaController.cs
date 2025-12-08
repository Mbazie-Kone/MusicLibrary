using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Api.Services;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Infrastructure.Repositories;

namespace MusicLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IMediaRepository _mediaRepository;

        public MediaController(IFileStorageService fileStorage, IMediaRepository mediaRepository)
        {
            _fileStorage = fileStorage;
            _mediaRepository = mediaRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0 )
                return BadRequest("No file uploaded.");

            // Save the file physically
            string storedFileName = await _fileStorage.SaveFileAsync(file);

            // Create the MediaItem entity
            var mediaItem = new MediaItem
            {
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                FileName = storedFileName,
                FileType = Path.GetExtension(file.FileName)?.Replace(".", ""),
                FileSize = file.Length
            };

            // Save the metadata in the database
            await _mediaRepository.Addsync(mediaItem);

            return Ok(new
            {
                message = "File uploaded successfully",
                file = mediaItem
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _mediaRepository.GetAllAsync();

            var result = items.Select(i => new
            {
                i.Id,
                i.Title,
                i.FileName,
                i.FileType,
                i.FileSize,
                i.UploadedAt
            });

            return Ok(result);
        }
       
    }
}
