namespace YetAnotherOllamaManager.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class BaseModelDetails
{
    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("family")]
    public string Family { get; set; }

    [JsonPropertyName("families")]
    public object Families { get; set; }

    [JsonPropertyName("parameter_size")]
    public string ParameterSize { get; set; }

    [JsonPropertyName("quantization_level")]
    public string QuantizationLevel { get; set; }
}

public class Model
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("modified_at")]
    public DateTime ModifiedAt { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }// in bytes

    [JsonIgnore]
    public string ReadableSize
    {
        get
        {
            const int scale = 1024;
            var orders = new[]
            {
                "TB",
                "GB",
                "MB",
                "KB",
                "Bytes"
            };
            var max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (Size > max)
                    return $"{decimal.Round((decimal)Size / (max), 2)} {order}";

                max /= scale;
            }
            return "0 Bytes";
        }
    }
    
    [JsonIgnore]
    public DateTime? LastUpdate { get; set; }

    [JsonIgnore]
    public bool ShouldUpdate => LastUpdate > ModifiedAt;

    [JsonPropertyName("digest")]
    public string Digest { get; set; }

    [JsonPropertyName("details")]
    public BaseModelDetails Details { get; set; }
}

public class GetModelListResponse
{
    [JsonPropertyName("models")]
    public List<Model> Models { get; set; }
}
