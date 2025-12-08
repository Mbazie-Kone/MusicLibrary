
using Minio;

namespace MusicLibrary.Api.Services
{
    public class MinioService : IMinioService
    {
        private readonly MinioClient _client;
        private readonly string _bucketName;

        public MinioService(IConfiguration config)
        {
            var endpoint = config["Minio:Endpoint"];
            var accessKey = config["Minio:AccessKey"];
            var secretKey = config["Minio:SecretKey"];
            _bucketName = config["Minio:BucketName"];

            _client = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(false)
                .Build();
        }

        public async Task EnsureBucketExistsAsync()
        {
            bool exists = await _client.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_bucketName)
            );

            if (!exists)
            {
                await _client.MakeBucketAsync(
                    new MakeBucketArgs().WithBucket(_bucketName)
                );
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            await EnsureBucketExistsAsync();

            string objectName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var stream = file.OpenReadStream())
            {
                await _client.PutObjectAsync(
                    new PutObjectArgs()
                        .WithBucket(_bucketName)
                        .WithObject(objectName)
                        .WithStreamData(stream)
                        .WithObjectSize(file.Length)
                        .WithContentType(file.ContentType)
                );
            }

            return objectName;
        }
    }
}
