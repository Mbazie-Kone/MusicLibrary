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

        // POST: api/Media/upload
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _mediaRepository.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // GET: api/Media/{id}/stream
        [HttpGet("{id:int}/stream")]
        public async Task<IActionResult> Stream(int id, CancellationToken cancellationToken)
        {
            // 400 -> invalid id
            if (id <= 0)
                return BadRequest("Invalid media ID.");

            // 404 -> not found
            var media = await _mediaRepository.GetByIdAsync(id);
            if (media == null)
                return NotFound();

            try
            {
                var (contentType, size) = await _minioService.GetObjectInfoAsync(media.FileName);

                Response.Headers.Add("Accept-Ranges", "bytes");

                if (size.HasValue)
                    Response.ContentLength = size.Value;

                Response.ContentType = contentType;

                // Direct streaming in the response body
                await _minioService.StreamObjectAsync(media.FileName, Response.Body, cancellationToken);

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                // 500 → storage or streaming failure
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while streaming media file.");
            }
        }

        // PUT: api/Media/{id}
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

        // DELETE: api/Media/{id}
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
