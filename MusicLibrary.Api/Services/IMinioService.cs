namespace MusicLibrary.Api.Services
{
    public interface IMinioService
    {
        Task EnsureBucketExistsAsync();
        Task<string> UploadFileAsync(IFormFile file);
    }
}
