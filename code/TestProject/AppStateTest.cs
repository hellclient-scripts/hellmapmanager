
using HellMapManager.States;
using HellMapManager.Models;
using HellMapManager;
namespace TestProject;

public class AppStateTest
{
    [Fact]
    public void TestBasic()
    {
        var appState = new AppState();
        var updated = false;
        appState.MapFileUpdatedEvent += (sender, e) =>
                {
                    updated = true;
                };
        Assert.Null(appState.Current);
        Assert.False(updated);
        var mf = new MapFile();
        appState.SetCurrent(mf);
        Assert.True(updated);

        updated = false;
        appState.CloseCurrent();
        Assert.Null(appState.Current);
        Assert.True(updated);
        appState.NewMap();
        updated = false;
        Assert.NotNull(appState.Current);
        Assert.False(updated);
        Assert.Equal("", appState.Current.Map.Info.Name);
        Assert.Equal("", appState.Current.Map.Info.Desc);
        Assert.Equal(MapEncoding.Default, appState.Current.Map.Encoding);
        updated = false;
        appState.UpdateMapSettings(new MapSettings()
        {
            Name = "name",
            Desc = "desc",
            Encoding = MapEncoding.GB18030,
        });
        Assert.True(updated);
        Assert.Equal("name", appState.Current.Map.Info.Name);
        Assert.Equal("desc", appState.Current.Map.Info.Desc);
        Assert.Equal(MapEncoding.GB18030, appState.Current.Map.Encoding);


        Assert.Equal(5, AppPreset.MaxRecents);
        Assert.Empty(appState.Settings.Recents);
        appState.AddRecent(new RecentFile("name1", "path1"));
        Assert.Single(appState.Settings.Recents);
        Assert.Equal("name1", appState.Settings.Recents[0].Name);
        Assert.Equal("path1", appState.Settings.Recents[0].Path);
        appState.AddRecent(new RecentFile("name1", "path1"));
        Assert.Single(appState.Settings.Recents);
        Assert.Equal("name1", appState.Settings.Recents[0].Name);
        Assert.Equal("path1", appState.Settings.Recents[0].Path);
        appState.AddRecent(new RecentFile("name2", "path2"));
        Assert.Equal(2, appState.Settings.Recents.Count);
        Assert.Equal("name2", appState.Settings.Recents[0].Name);
        Assert.Equal("path2", appState.Settings.Recents[0].Path);
        Assert.Equal("name1", appState.Settings.Recents[1].Name);
        Assert.Equal("path1", appState.Settings.Recents[1].Path);
        appState.AddRecent(new RecentFile("name1", "path1"));
        Assert.Equal(2, appState.Settings.Recents.Count);
        Assert.Equal("name1", appState.Settings.Recents[0].Name);
        Assert.Equal("path1", appState.Settings.Recents[0].Path);
        Assert.Equal("name2", appState.Settings.Recents[1].Name);
        Assert.Equal("path2", appState.Settings.Recents[1].Path);

        appState.Settings = new Settings();
        for (var i = 0; i < AppPreset.MaxRecents + 1; i++)
        {
            appState.AddRecent(new RecentFile($"name{i}", $"path{i}"));
        }
        Assert.Equal(AppPreset.MaxRecents, appState.Settings.Recents.Count);
        Assert.Equal("name" + AppPreset.MaxRecents.ToString(), appState.Settings.Recents[0].Name);
        Assert.Equal("path" + AppPreset.MaxRecents.ToString(), appState.Settings.Recents[0].Path);
        Assert.Equal("name1", appState.Settings.Recents[AppPreset.MaxRecents - 1].Name);
        Assert.Equal("path1", appState.Settings.Recents[AppPreset.MaxRecents - 1].Path);
    }

    [Fact]
    public void TestEvent()
    {
        var mfupdated=false;
        var settingsupdated=false;
        var exited=false;
        var appState = new AppState();
        appState.MapFileUpdatedEvent += (sender, e) =>
                {
                    mfupdated = true;
                };
        appState.SettingsUpdatedEvent += (sender, e) =>
                {
                    settingsupdated = true;
                };
        appState.ExitEvent += (sender, e) =>
                {
                    exited = true;
                };
        appState.RaiseMapFileUpdatedEvent(this);
        Assert.True(mfupdated);
        appState.RaiseSettingsUpdatedEvent(this);
        Assert.True(settingsupdated);
        appState.RaiseExitEvent(this);
        Assert.True(exited);
    }
}