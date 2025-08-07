using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        if (config == null) throw new NullReferenceException($"Tried to read invalid config: {configJson}");
        config[key] = value;
        var updatedConfigJson = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText("appsettings.json", updatedConfigJson);
    }
    public static DateTime? GetSecondUpdatedDateTime(this string input)
    {
        //replaced with alternate code below
        //if(input.Contains("Updated yesterday", StringComparison.InvariantCultureIgnoreCase)) return DateTime.Today.AddDays(-1);

        //var matches = MyRegex().Matches(input);
        //if (matches.Count < 1)
        //    return null;

        //var secondMatch = matches[0];
        //var valueNeedsCorrection = secondMatch.Groups[1].Value is "an" or "a";
        //var quantity = int.Parse(valueNeedsCorrection ? "1" : secondMatch.Groups[1].Value);
        //var unit = secondMatch.Groups[2].Value;
        //if (!unit.EndsWith('s')) unit += "s";

        //var now = DateTime.Now;

        //return unit.ToLower() switch
        //{
        //    "minutes" => now.AddMinutes(-quantity),
        //    "seconds" => now.AddSeconds(-quantity),
        //    "hours" => now.AddHours(-quantity),
        //    "days" => now.AddDays(-quantity),
        //    "weeks" => now.AddDays(-quantity * 7),
        //    "months" => now.AddMonths(-quantity),
        //    "years" => now.AddYears(-quantity),
        //    _ => null
        //};
        DateTime rdate;
        if (input.Contains("<span class=\"flex items-center\" title=\"", StringComparison.InvariantCultureIgnoreCase))
        {
            string formatSpecifier = "MMM d, yyyy h:mm tt 'UTC'";
            string findupdate = "<span class=\"flex items-center\" title=\"";            
            var ind = input.IndexOf(findupdate);
            var ind2 = input.IndexOf("\"", ind + findupdate.Length);
            var sdate = input.Substring(ind + findupdate.Length, ind2 - (ind + findupdate.Length));            
            if (DateTime.TryParseExact(sdate, new[] { formatSpecifier }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out rdate) == true)
                return rdate;
            else
                return null;
        }
        else return null;
            
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
