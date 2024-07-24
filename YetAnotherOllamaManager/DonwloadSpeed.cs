using System;
using System.Collections.Generic;
using System.Linq;

namespace YetAnotherOllamaManager;

public class DownloadSpeed
{
    private DateTime _lastUpdateTime;
    private long _lastDownloaded;
    private readonly Queue<(double SpeedMBps, TimeSpan EstimatedTime)> _dataPoints = new(50);

    public (double SmoothedSpeedMBps, TimeSpan SmoothedEstimatedTime) UpdateDownloadProgress(long totalSize, long downloaded, DateTime currentTime)
    {
        if (_lastUpdateTime == default || _lastDownloaded == 0)
        {
            _lastUpdateTime = currentTime;
            _lastDownloaded = downloaded;
            return (0.0, TimeSpan.Zero);
        }

        var bytesDownloaded = downloaded - _lastDownloaded;
        var elapsedSeconds = (currentTime - _lastUpdateTime).TotalSeconds;

        if (elapsedSeconds == 0) // Avoid division by zero
            return (_dataPoints.Average(dp => dp.SpeedMBps), TimeSpan.Zero);

        var downloadSpeedBytesPerSecond = bytesDownloaded / elapsedSeconds;

        var averageDownloadSpeedMBps = downloadSpeedBytesPerSecond / (1024 * 1024);

        var remainingMegaBytes = (totalSize - downloaded) / (1024 * 1024);
        if (averageDownloadSpeedMBps == 0) // Avoid division by zero
            return (averageDownloadSpeedMBps, TimeSpan.MaxValue);

        var estimatedRemainingSeconds = remainingMegaBytes / averageDownloadSpeedMBps;
        var estimatedRemainingTime = TimeSpan.FromSeconds(estimatedRemainingSeconds);

        _lastUpdateTime = currentTime;
        _lastDownloaded = downloaded;

        _dataPoints.Enqueue((averageDownloadSpeedMBps, estimatedRemainingTime));
        if (_dataPoints.Count > 50)
            _dataPoints.Dequeue();

        // Calculate smoothed values by taking the average over the last 10 data points:
        var smoothedAverageDownloadSpeedMBps = _dataPoints.Average(dp => dp.SpeedMBps);
        var smoothedEstimatedRemainingTime = TimeSpan.FromSeconds(_dataPoints.Average(dp => dp.EstimatedTime.TotalSeconds));

        return (smoothedAverageDownloadSpeedMBps, smoothedEstimatedRemainingTime);
    }
}