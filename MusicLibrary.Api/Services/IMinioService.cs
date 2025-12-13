namespace MusicLibrary.Api.Services
{
    public interface IMinioService
    {
        Task EnsureBucketExistsAsync();
        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(string fileName);

        Task StreamObjectAsync(
            string objectName,
            Stream destination,
            CancellationToken cancellationToken);

        Task<(string ContentType, long? Size)> GetObjectInfoAsync(string objectName);

    }
}
