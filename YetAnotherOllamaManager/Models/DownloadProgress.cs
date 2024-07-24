namespace YetAnotherOllamaManager.Models;

using System.Text.Json.Serialization;

public class DownloadProgress
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("digest")]
    public string? Digest { get; set; }
    [JsonPropertyName("total")]
    public long? Total { get; set; }
    [JsonPropertyName("completed")]
    public long? Completed { get; set; }
    [JsonIgnore]
    public float CompletedPercentage => Completed == null || Total == null ? 0.0f : (float)((double)Completed / (double)Total * 100);
}
