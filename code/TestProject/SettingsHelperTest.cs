using HellMapManager.Models;
using HellMapManager.Services;

namespace TestProject;

public class SettingsHelperTest
{
    [Fact]
    public void TestSettings()
    {
        var settings = new Settings();
        settings.APIEnabled = true;
        settings.APIUserName = "user";
        settings.APIPassWord = "password";
        settings.Recents = [
            new RecentFile("test1", "test1"),
            new RecentFile("test2", "test2"),
            new RecentFile("test3", "test3"),
        ];
        var settings2=SettingsHelper.FromJSON(SettingsHelper.ToJSON(settings));
        Assert.NotNull(settings2);
        Assert.Equal(settings.APIEnabled, settings2.APIEnabled);
        Assert.Equal(settings.APIUserName, settings2.APIUserName);
        Assert.Equal(settings.APIPassWord, settings2.APIPassWord);
        Assert.Equal(settings.Recents.Count, settings2.Recents.Count);
        for (int i = 0; i < settings.Recents.Count; i++)
        {
            Assert.Equal(settings.Recents[i].Name, settings2.Recents[i].Name);
            Assert.Equal(settings.Recents[i].Path, settings2.Recents[i].Path);
        }
    }
}