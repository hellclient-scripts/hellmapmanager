using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HellMapManager.Models;

[JsonSerializable(typeof(MapInfo))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(int))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(List<RecentFile>))]
public partial class SettingsContext : JsonSerializerContext
{
}

public class Settings
{
    public static string CurrentVersion = "1.0";
    public List<RecentFile> Recents = [];
    public string BindPort = "8466";
    public string APIUserName = "";
    public string APIPassWord = "";
    public bool APIEnabled = false;
    public string ToJSON()
    {
        var jsonString = JsonSerializer.Serialize(this, typeof(Settings), SettingsContext.Default);
        return jsonString;
    }
}