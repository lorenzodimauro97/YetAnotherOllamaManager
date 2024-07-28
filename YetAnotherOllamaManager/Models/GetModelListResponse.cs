namespace YetAnotherOllamaManager.Models;

using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ExtendedModel : Model
{
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
}
