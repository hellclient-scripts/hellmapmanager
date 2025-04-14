using System;
using System.IO;
using HellMapManager.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;
using System.Text.Encodings.Web;

namespace HellMapManager.Services;

[JsonSerializable(typeof(Settings))]
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
    public void Save(Settings settings)
    {
        if (SettingPath == "")
        {
            return;
        }
        using var fileStream = new FileStream(SettingPath, FileMode.Create);
        using var sw = new StreamWriter(fileStream, Encoding.UTF8);
        sw.Write(ToJSON(settings));
    }
    public static string ToJSON(Settings settings)
    {
        var opt = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true
        };
        var jsonString = JsonSerializer.Serialize(settings, typeof(Settings), new SettingsContext(opt));
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
    public Settings? Load()
    {
        if (SettingPath == "" || !File.Exists(this.SettingPath))
        {
            return null;
        }
        using var fileStream = new FileStream(SettingPath, FileMode.Open);
        using var sr = new StreamReader(fileStream, Encoding.UTF8);
        var body = sr.ReadToEnd();
        return FromJSON(body);

    }
}