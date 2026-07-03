using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PartsManager.Api.Services
{
    public class BackupBackgroundService : BackgroundService
    {
        private readonly ILogger<BackupBackgroundService> _logger;
        private readonly GoogleDriveService _driveService;
        private readonly int _intervalHours;
        private readonly int _maxRetention;
        private readonly string _dbPath;
        private readonly string _attachmentsPath;

        public BackupBackgroundService(ILogger<BackupBackgroundService> logger, GoogleDriveService driveService, IConfiguration config)
        {
            _logger = logger;
            _driveService = driveService;
            
            _intervalHours = int.TryParse(config["Backup:BackupIntervalHours"], out int h) ? h : 24;
            _maxRetention = int.TryParse(config["Backup:MaxRetentionCount"], out int r) ? r : 7;
            
            string connStr = config.GetConnectionString("DefaultConnection") ?? "";
            _dbPath = connStr.Replace("Data Source=", "").Trim();
            _attachmentsPath = config["System:AttachmentPath"] ?? "Attachments";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_driveService.IsConfigured)
            {
                _logger.LogWarning("Google Drive is not configured. BackupBackgroundService will not run.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting scheduled backup...");
                    await _driveService.UploadBackupAsync(_dbPath, _attachmentsPath);
                    await _driveService.DeleteOldBackupsAsync(_maxRetention);
                    _logger.LogInformation("Scheduled backup completed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Scheduled backup failed: {ex.Message}");
                }

                await Task.Delay(TimeSpan.FromHours(_intervalHours), stoppingToken);
            }
        }
    }
}
