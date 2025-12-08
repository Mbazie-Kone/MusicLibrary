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

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider, IConfiguration config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

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
                    .FirstOrDefaultAsync(stoppingToken);

                if (item != null)
                {
                    await Task.Delay(3000, stoppingToken); // Simulate processing time
                    continue;
                }

                _logger.LogInformation($"Processing media: {item.Id} {item.FileName}");

                item.Status = MediaStatus.Processed;
                await db.SaveChangesAsync();

                var tempPath = Path.GetTempFileName();
                await DownloadFromMinio(item.FileName, tempPath);

                ExtractMetadata(item, tempPath);

                item.Status = MediaStatus.Processed;
                await db.SaveChangesAsync();

                File.Delete(tempPath);

                _logger.LogInformation($"Processed media: {item.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing media: {ex.Message}");

            }
        }
    }

    private async Task DownloadFromMinio(string objectName, string filePath)
    {
    await _minio.GetObjectAsync(new GetObjectArgs()
        .WithBucket("media")
        .WithObject(objectName)
        .WithFile(filePath));
    }

    private void ExtractMetadata(MediaItem item, string filePath)
    {
        var tagFile = TagLib.File.Create(filePath);

        // Simulate metadata extraction
        item.Duration = tagFile.Properties.Duration.ToString();
        item.Bitrate = tagFile.Properties.AudioBitrate.ToString();
        item.Artist = tagFile.Tag.FirstPerformer ?? "Unknown";
        item.Album = tagFile.Tag.Album ?? "Unknown";
    }
}
}