namespace HellMapManager.Models;

public class ExternalLink(string name, string link, string intro)
{
    public string Name { get; set; } = name;
    public string Link { get; set; } = link;
    public string Intro { get; set; } = intro;
}
