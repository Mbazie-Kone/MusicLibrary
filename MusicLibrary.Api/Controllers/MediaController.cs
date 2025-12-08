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
        private readonly IFileStorageService _fileStorage;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMinioService _minioService;

        public MediaController(IFileStorageService fileStorage, IMediaRepository mediaRepository, IMinioService minioService)
        {
            _fileStorage = fileStorage;
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
                // If file deletion fails, abort the operation to maintain consistency
                // between MinIO storage and database records
                throw; 
            }

            await _mediaRepository.DeleteAsync(id);

            return NoContent();
        }
       
    }
}
