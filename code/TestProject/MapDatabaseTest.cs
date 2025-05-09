
using HellMapManager.Cores;
using HellMapManager.Models;
namespace TestProject;

public class MapDatabaseTest
{
    [Fact]
    public void TestBasic()
    {
        var mapDatabase = new MapDatabase();
        var updated = false;
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
                {
                    updated = true;
                };
        Assert.Null(mapDatabase.Current);
        Assert.False(updated);
        var mf = new MapFile();
        mapDatabase.SetCurrent(mf);
        Assert.True(updated);

        updated = false;
        mapDatabase.CloseCurrent();
        Assert.Null(mapDatabase.Current);
        Assert.True(updated);
        mapDatabase.NewMap();
        updated = false;
        Assert.NotNull(mapDatabase.Current);
        Assert.False(updated);
        Assert.Equal("", mapDatabase.Current.Map.Info.Name);
        Assert.Equal("", mapDatabase.Current.Map.Info.Desc);
        Assert.Equal(MapEncoding.Default, mapDatabase.Current.Map.Encoding);
        updated = false;
        mapDatabase.UpdateMapSettings(new MapSettings()
        {
            Name = "name",
            Desc = "desc",
            Encoding = MapEncoding.GB18030,
        });
        Assert.True(updated);
        Assert.Equal("name", mapDatabase.Current.Map.Info.Name);
        Assert.Equal("desc", mapDatabase.Current.Map.Info.Desc);
        Assert.Equal(MapEncoding.GB18030, mapDatabase.Current.Map.Encoding);


        Assert.Equal(5, AppPreset.MaxRecents);
        Assert.Empty(mapDatabase.Settings.Recents);
        mapDatabase.AddRecent(new RecentFile("name1", "path1"));
        Assert.Single(mapDatabase.Settings.Recents);
        Assert.Equal("name1", mapDatabase.Settings.Recents[0].Name);
        Assert.Equal("path1", mapDatabase.Settings.Recents[0].Path);
        mapDatabase.AddRecent(new RecentFile("name1", "path1"));
        Assert.Single(mapDatabase.Settings.Recents);
        Assert.Equal("name1", mapDatabase.Settings.Recents[0].Name);
        Assert.Equal("path1", mapDatabase.Settings.Recents[0].Path);
        mapDatabase.AddRecent(new RecentFile("name2", "path2"));
        Assert.Equal(2, mapDatabase.Settings.Recents.Count);
        Assert.Equal("name2", mapDatabase.Settings.Recents[0].Name);
        Assert.Equal("path2", mapDatabase.Settings.Recents[0].Path);
        Assert.Equal("name1", mapDatabase.Settings.Recents[1].Name);
        Assert.Equal("path1", mapDatabase.Settings.Recents[1].Path);
        mapDatabase.AddRecent(new RecentFile("name1", "path1"));
        Assert.Equal(2, mapDatabase.Settings.Recents.Count);
        Assert.Equal("name1", mapDatabase.Settings.Recents[0].Name);
        Assert.Equal("path1", mapDatabase.Settings.Recents[0].Path);
        Assert.Equal("name2", mapDatabase.Settings.Recents[1].Name);
        Assert.Equal("path2", mapDatabase.Settings.Recents[1].Path);

        mapDatabase.Settings = new Settings();
        for (var i = 0; i < AppPreset.MaxRecents + 1; i++)
        {
            mapDatabase.AddRecent(new RecentFile($"name{i}", $"path{i}"));
        }
        Assert.Equal(AppPreset.MaxRecents, mapDatabase.Settings.Recents.Count);
        Assert.Equal("name" + AppPreset.MaxRecents.ToString(), mapDatabase.Settings.Recents[0].Name);
        Assert.Equal("path" + AppPreset.MaxRecents.ToString(), mapDatabase.Settings.Recents[0].Path);
        Assert.Equal("name1", mapDatabase.Settings.Recents[AppPreset.MaxRecents - 1].Name);
        Assert.Equal("path1", mapDatabase.Settings.Recents[AppPreset.MaxRecents - 1].Path);
    }

    [Fact]
    public void TestEvent()
    {
        var mfupdated=false;
        var settingsupdated=false;
        var exited=false;
        var mapDatabase = new MapDatabase();
        mapDatabase.MapFileUpdatedEvent += (sender, e) =>
                {
                    mfupdated = true;
                };
        mapDatabase.SettingsUpdatedEvent += (sender, e) =>
                {
                    settingsupdated = true;
                };
        mapDatabase.ExitEvent += (sender, e) =>
                {
                    exited = true;
                };
        mapDatabase.RaiseMapFileUpdatedEvent(this);
        Assert.True(mfupdated);
        mapDatabase.RaiseSettingsUpdatedEvent(this);
        Assert.True(settingsupdated);
        mapDatabase.RaiseExitEvent(this);
        Assert.True(exited);
    }
}