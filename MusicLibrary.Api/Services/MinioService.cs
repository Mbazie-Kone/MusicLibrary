using Minio;

namespace MusicLibrary.Api.Services
{
    public class MinioService : IMinioService
    {
        private readonly MinioClient _client;
        private readonly string _bucket;

        public MinioService(IConfiguration config)
        {
            _bucket = config["Minio:BucketName"]!;

            _client = new MinioClient()
               .WithEndpoint(config["Minio:Endpoint"])
               .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
               .WithSSL(false)
               .Build();

        }

        public async Task EnsureBucketExistsAsync()
        {
            bool exists = await _client.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucket)
            );

            if (!exists)
            {
                await _client.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucket)
                );
            }
        }

        public async Task<(string ContentType, long Size)> GetObjectInfoAsync(string objectName)
        {
            var stat = await _client.StatObjectAsync(
                new StatObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectName));

            return (
                stat.ContentType ?? "application/octet-stream",
                stat.Size
            );
        }

        public async Task StreamObjectAsync(string objectName, Stream destination)
        {
            await _client.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectName)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(destination);
                    })

            );
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            await EnsureBucketExistsAsync();

            string objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                await _client.PutObjectAsync(
                    new PutObjectArgs()
                        .WithBucket(_bucket)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(file.Length)
                        .WithContentType(file.ContentType)
                );
            }

            return objectName;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _client.RemoveObjectAsync(
                new RemoveObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(fileName)
            );
        }

        public async Task<Stream> GetObjectStreamAsync(string objectName)
        {
            var memoryStream = new MemoryStream();

            await _client.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_bucket)
                    .WithObject(objectName)
                    .WithCallbackStream(stream =>
                    {
                        stream.CopyTo(memoryStream);
                    })
            );

            memoryStream.Position = 0;
            return memoryStream;
        }

    }
}