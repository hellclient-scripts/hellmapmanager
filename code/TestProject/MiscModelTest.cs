using HellMapManager.Models;
namespace TestProject;

public class MiscModelTest
{
    [Fact]
    public void TestModel()
    {
        var cache = new Cache();
        var env = new HellMapManager.Models.Environment();
        var path = new HellMapManager.Models.Path();

    }
    [Fact]
    public void TestSettings()
    {
        Assert.Equal("1.0", Settings.CurrentVersion);
        var settings = new Settings();
        Assert.False(settings.APIEnabled);
        Assert.Equal(8466, settings.APIPort);
        Assert.Equal("", settings.APIUserName);
        Assert.Equal("", settings.APIPassWord);
        Assert.Empty(settings.Recents);
    }
    [Fact]
    public void TestExternalLink()
    {
        var link = new ExternalLink("name", "link", "intro");
        Assert.Equal("name", link.Name);
        Assert.Equal("link", link.Link);
        Assert.Equal("intro", link.Intro);

    }
    [Fact]
    public void TestRecentFile()
    {
        var recent = new RecentFile("", "");
        Assert.Equal("", recent.Name);
        Assert.Equal("", recent.Path);
        Assert.Equal("<未命名> ", recent.Label);
        Assert.Equal($"地图名:\n地图文件路径:", recent.Detail);
        recent = new RecentFile("name", "path");
        Assert.Equal("name", recent.Name);
        Assert.Equal("path", recent.Path);
        Assert.Equal("name path", recent.Label);
        Assert.Equal($"地图名name:\n地图文件路径:path", recent.Detail);
    }
}
