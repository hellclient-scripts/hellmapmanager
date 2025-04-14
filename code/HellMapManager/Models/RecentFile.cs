using System.Text.Json.Serialization;

namespace HellMapManager.Models;

public class RecentFile(string name, string path)
{
    public string Name { get; set; } = name;
    public string Path { get; set; } = path;
    [JsonIgnore]
    public string Detail
    {
        get => $"地图名{Name}:\n地图文件路径:{Path}";
    }
    [JsonIgnore]
    public string Label
    {
        get => (Name == "" ? "<未命名>" : Name) + " " + Path;
    }
}