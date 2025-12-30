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
        Assert.Equal(Settings.DefaultAPIPort, settings.APIPort);
        Assert.Equal("", settings.APIUserName);
        Assert.Equal("", settings.APIPassWord);
        Assert.Empty(settings.Recents);
        Assert.Equal($"http://localhost:{Settings.DefaultAPIPort}/", settings.BuildURL());
        settings.APIPort = 1234;
        Assert.Equal(1234, settings.GetPort());
        settings.Host = "0.0.0.0";
        Assert.Equal($"http://0.0.0.0:1234/", settings.BuildURL());
        settings.APIPort = -1;
        Assert.Equal(Settings.DefaultAPIPort, settings.GetPort());

    }
    [Fact]
    public void TestAPIConfig()
    {
        var settings = new Settings()
        {
            APIPort = 1234,
            APIUserName = "user",
            APIPassWord = "pass",
            APIEnabled = true,
        };
        var apiConfig = APIConfig.From(settings);
        Assert.Equal(1234, apiConfig.APIPort);
        Assert.Equal("user", apiConfig.APIUserName);
        Assert.Equal("pass", apiConfig.APIPassWord);
        Assert.True(apiConfig.APIEnabled);
        var newSettings = new Settings();
        apiConfig.Apply(newSettings);
        Assert.Equal(1234, newSettings.APIPort);
        Assert.Equal("user", newSettings.APIUserName);
        Assert.Equal("pass", newSettings.APIPassWord);
        Assert.True(newSettings.APIEnabled);
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
