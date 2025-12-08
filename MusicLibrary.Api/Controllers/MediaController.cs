using Microsoft.AspNetCore.Mvc;
using MusicLibrary.Api.Services;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Infrastructure.Repositories;
using MusicLibrary.Api.Dtos;

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

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0 )
                return BadRequest("No file uploaded.");

            // Save the file physically
            string storedFileName = await _minioService.UploadFileAsync(file);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMediaItemRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            item.Title = request.Title;
            
            await _mediaRepository.UpdateAsync(item);
            return Ok(item);
        }

        [HttpDelete("{id}")]
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
                // In a real app, we might want to log this but still proceed to delete metadata, 
                // or fail. For now, we proceed to ensure DB consistency or fail? 
                // Usually we want DB to reflect reality. If file is gone, DB should be gone. 
                // If file delete fails, maybe we shouldn't delete DB record?
                // Let's assume strict consistency: if file delete fails, abort.
                throw; 
            }

            await _mediaRepository.DeleteAsync(id);

            return NoContent();
        }
       
    }
}
