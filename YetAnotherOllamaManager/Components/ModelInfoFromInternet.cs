namespace YetAnotherOllamaManager.Components;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ModelFromInternet
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }
}

public class ModelInfoFromInternet
{
    [JsonPropertyName("models")]
    public List<ModelFromInternet> Models { get; set; }
}