using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Api.Dtos;
using MusicLibrary.Api.Services;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Enums;
using MusicLibrary.Infrastructure.Repositories;

namespace MusicLibrary.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IMinioService _minioService;

        public MediaController(IMediaRepository mediaRepository, IMinioService minioService)
        {
            _mediaRepository = mediaRepository;
            _minioService = minioService;
        }

        // POST: api/Media/upload
        [RequestSizeLimit(1073741824)] // 1 GB limit
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Save the file physically
            string storedFileName = await _minioService.UploadFileAsync(file);

            // Create the MediaItem entity
            var mediaItem = new MediaItem
            {
                Title = Path.GetFileNameWithoutExtension(file.FileName),
                FileName = storedFileName,
                FileType = Path.GetExtension(file.FileName)?.Replace(".", ""),
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow,
                Status = MediaStatus.Pending
            };

            // Save the metadata in the database
            await _mediaRepository.AddAsync(mediaItem);

            return Ok(new
            {
                success = true,
                id = mediaItem.Id
            });
        }

        // GET: api/Media
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

        // GET: api/Media/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // GET: api/Media/{id}/stream
        [HttpGet("{id:int}/stream")]
        public async Task<IActionResult> Stream(int id)
        {

            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            var (contentType, _) = await _minioService.GetObjectInfoAsync(media.FileName);

            var memory = new MemoryStream();

            await _minioService.StreamObjectAsync(media.FileName, memory);
            memory.Position = 0;


            return File(memory, contentType ?? "audio/mpeg", enableRangeProcessing: true);

        }

        // PUT: api/Media/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMediaItemRequest request)
        {

            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            item.Title = request.Title;

            await _mediaRepository.UpdateAsync(item);
            return Ok(item);
        }

        // DELETE: api/Media/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            // Delete physical file (best effort, log if fails?)
            try
            {
                await _minioService.DeleteFileAsync(item.FileName);
            }
            catch (Exception)
            {
                // If file deletion fails, abort the operation to maintain consistency
                // between MinIO storage and database records
                throw;
            }

            await _mediaRepository.DeleteAsync(id);

            return NoContent();
        }

    }
}