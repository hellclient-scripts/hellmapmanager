using System.IO;
using HellMapManager.Models;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using HellMapManager.Adapters;
using System;
namespace HellMapManager.Helpers;

[JsonSerializable(typeof(Settings))]
[JsonSerializable(typeof(bool))]
// [JsonSerializable(typeof(int))]
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
        try
        {

            if (SettingPath == "")
            {
                return;
            }
            using var fileStream = SystemAdapter.Instance.File.WriteStream(SettingPath);
            using var sw = new StreamWriter(fileStream, Encoding.UTF8);
            sw.Write(ToJSON(settings));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
        try
        {
            if (SettingPath == "" || !SystemAdapter.Instance.File.Exists(this.SettingPath))
            {
                return null;
            }
            using var fileStream = SystemAdapter.Instance.File.ReadStream(SettingPath);
            using var sr = new StreamReader(fileStream, Encoding.UTF8);
            var body = sr.ReadToEnd();
            return FromJSON(body);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Settings();
        }
    }
}