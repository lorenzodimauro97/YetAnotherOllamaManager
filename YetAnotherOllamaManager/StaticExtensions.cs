using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace YetAnotherOllamaManager;

public static partial class StaticExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };
    public static void UpdateAppSetting(string key, string value)
    {
        var configJson = File.ReadAllText("appsettings.json");
        var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
        config[key] = value;
        var updatedConfigJson = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText("appsettings.json", updatedConfigJson);
    }
    public static DateTime? GetSecondUpdatedDateTime(this string input)
    {
        if(input.Contains("Updated yesterday", StringComparison.InvariantCultureIgnoreCase)) return DateTime.Today.AddDays(-1);
        
        var matches = MyRegex().Matches(input);
        if (matches.Count < 2)
            return null;

        var secondMatch = matches[1];
        var valueNeedsCorrection = secondMatch.Groups[1].Value is "an" or "a";
        var quantity = int.Parse(valueNeedsCorrection ? "1" : secondMatch.Groups[1].Value);
        var unit = secondMatch.Groups[2].Value;
        if (!unit.EndsWith('s')) unit += "s";

        var now = DateTime.Now;

        return unit.ToLower() switch
        {
            "minutes" => now.AddMinutes(-quantity),
            "seconds" => now.AddSeconds(-quantity),
            "hours" => now.AddHours(-quantity),
            "days" => now.AddDays(-quantity),
            "weeks" => now.AddDays(-quantity * 7),
            "months" => now.AddMonths(-quantity),
            _ => null
        };
    }

    [GeneratedRegex(@"Updated (\d+) (\w+) ago")]
    private static partial Regex MyRegex();
    
    public static string ConvertToSize(this string? text)
    {
        var result = long.TryParse(text, out var size);
        if (result == false) return string.Empty;
        
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
                if (size > max)
                    return $"{decimal.Round((decimal)size / (max), 2)} {order}";

                max /= scale;
            }
            return "0 Bytes";
    }
}
