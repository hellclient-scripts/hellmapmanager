namespace HellMapManager.Models;

public class RecentFile
{
    public RecentFile(string name, string path)
    {
        Name = name;
        Path = path;
    }
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public string Detail
    {
        get => $"地图名{Name}:\n地图文件路径:{Path}";
    }
    public string Label
    {
        get => (this.Name == "" ? "<未命名>" : this.Name) + " " + this.Path;
    }
}