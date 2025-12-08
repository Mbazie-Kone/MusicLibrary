namespace MusicLibrary.Api.Services
{
    public interface IMinioService
    {
        Task EnsureBucketExistsAsync();

        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(string fileName);

        Task<(string ContentType, long Size)> GetObjectInfoAsync(string objectName);

        Task<Stream> GetObjectStreamAsync(string objectName);

        Task StreamObjectAsync(string objectName, Stream destination);

    }
}