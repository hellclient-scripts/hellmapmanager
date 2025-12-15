using HellMapManager.Models;
using HellMapManager.Helpers;
namespace HellMapManager.Cores;


public partial class MapDatabase()
{
    private object _lock = new();
    public const int Version = 1001;
    public MapFile? Current;
    public Settings Settings = new();

    public void OpenRecent(string file)
    {
        if (file != "")
        {
            LoadFile(file);
        }
    }

    public void Save()
    {
        if (Current != null)
        {
            SaveFile(Current.Path);
        }
    }
    public void Revert()
    {
        if (Current != null && Current.Path != "")
        {
            LoadFile(Current.Path);
        }
    }
    public void AddRecent(RecentFile recent)
    {
        if (Settings.Recents.Count > 0 && Settings.Recents[0].Path == recent.Path && Settings.Recents[0].Name == recent.Name)
        {
            return;
        }
        Settings.Recents.RemoveAll(r => r.Path == recent.Path);
        Settings.Recents.Insert(0, recent);
        if (Settings.Recents.Count > AppPreset.MaxRecents)
        {
            Settings.Recents = Settings.Recents.GetRange(0, AppPreset.MaxRecents);
        }
        RaiseSettingsUpdatedEvent(this);
    }
    public void LoadFile(string file)
    {
        var mf = HMMFile.Open(file);
        if (mf != null)
        {
            lock (_lock)
            {
                Current = mf;
                Current.Modified = false;
                Current.Path = file;
                AddRecent(Current.ToRecentFile());
                RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public void SaveFile(string file)
    {
        if (Current != null)
        {
            lock (_lock)
            {

                Current.Map.Arrange();
                HMMFile.Save(file, Current);
                Current.Modified = false;
                Current.Path = file;
                AddRecent(Current.ToRecentFile());
                RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public Diffs? DiffFile(string file)
    {
        if (Current != null)
        {
            lock (_lock)
            {

                var mf = HMMFile.Open(file);
                if (mf != null)
                {
                    var diffs = DiffHelper.Diff(Current.Map, mf.Map);
                    return diffs;
                }
            }
        }
        return null;
    }
    public void Exit()
    {
        RaiseExitEvent(this);
    }

    public void NewMap()
    {
        lock (_lock)
        {
            var mapfile = MapFile.Create("", "");
            SetCurrent(mapfile);
        }
    }
    public void SetCurrent(MapFile mapfile)
    {
        lock (_lock)
        {
            Current = mapfile;
        }
        RaiseMapFileUpdatedEvent(this);
    }
    public void CloseCurrent()
    {
        lock (_lock)
        {
            Current = null;
        }
        RaiseMapFileUpdatedEvent(this);
    }
    public void UpdateMapSettings(MapSettings s)
    {
        if (Current != null)
        {
            lock (_lock)
            {
                Current.Map.Encoding = s.Encoding;
                Current.Map.Info.Name = s.Name;
                Current.Map.Info.Desc = s.Desc;
                Current.MarkAsModified();
                RaiseMapFileUpdatedEvent(this);
            }
        }
    }
}