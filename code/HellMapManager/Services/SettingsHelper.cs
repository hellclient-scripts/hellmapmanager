using System;
using System.IO;
using HellMapManager.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using HellMapManager.States;

namespace HellMapManager.Services;

[JsonSerializable(typeof(MapInfo))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(List<RecentFile>))]
public partial class SettingsContext : JsonSerializerContext
{
}
public class SettingsHelper(string setingpath)
{
    public string SettingPath = setingpath;
    public void WriteSettingsFile(object? sender, EventArgs args)
    {
    }
    public static string ToJSON(Settings settings)
    {
        var jsonString = JsonSerializer.Serialize(settings, typeof(Settings), SettingsContext.Default);
        return jsonString;
    }
    public static Settings? FromJSON(string val)
    {
        var data = JsonSerializer.Deserialize(val, typeof(Settings), SettingsContext.Default);
        if (data is Settings s)
        {
            return s;
        }
        return null;
    }
}