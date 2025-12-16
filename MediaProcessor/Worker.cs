using Microsoft.EntityFrameworkCore;
using Minio;
using MusicLibrary.Domain.Entities;
using MusicLibrary.Domain.Enums;
using MusicLibrary.Infrastructure.DbContexts;

namespace MediaProcessor { 

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly MinioClient _minio;
        private readonly string _bucketName;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration config)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _bucketName = config["Minio:BucketName"] ?? "media";

            _minio = new MinioClient()
                .WithEndpoint(config["Minio:Endpoint"])
                .WithCredentials(config["Minio:Accesskey"], config["Minio:SecretKey"])
                .WithSSL(false)
                .Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MediaProcessor Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<MusicLibraryDbContext>();

                    var item = await db.MediaItems
                        .Where(m => m.Status == MediaStatus.Pending)
                        .OrderBy(m => m.UploadedAt)
                        .FirstOrDefaultAsync(stoppingToken);

                    if (item == null)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                        continue;
                    }

                    _logger.LogInformation("Processing media {Id} ({File})", item.Id, item.FileName);

                    // Set Processing
                    item.Status = MediaStatus.Processed;
                    await db.SaveChangesAsync(stoppingToken);

                    // Download file from Minio
                    var tempPath = Path.GetTempFileName();
                    await DownloadFromMinio(item.FileName, tempPath, stoppingToken);

                    // Extract metadata
                    ExtractMetadata(item, tempPath);

                    item.Status = MediaStatus.Processed;
                    await db.SaveChangesAsync(stoppingToken);

                    File.Delete(tempPath);

                    _logger.LogInformation("Processed media {Id}", item.Id);
                }
                catch (OperationCanceledException)
                {
                    // graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "MediaProcessor iteration failed");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }  
            }
        }

        private async Task DownloadFromMinio(string objectName, string filePath, CancellationToken ct)
        {
            await _minio.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithFile(filePath),
                ct);
        }

        private void ExtractMetadata(MediaItem item, string filePath)
        {
            using var tagFile = TagLib.File.Create(filePath);

            // Simulate metadata extraction
            item.Duration = tagFile.Properties.Duration.ToString();
            item.Bitrate = tagFile.Properties.AudioBitrate.ToString();
            item.Artist = tagFile.Tag.FirstPerformer ?? "Unknown";
            item.Album = tagFile.Tag.Album ?? "Unknown";
        }
    }
}