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
    public string Label
    {
        get => (this.Name == "" ? "<未命名>" : this.Name) + " " + this.Path;
    }
}