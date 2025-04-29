using HellMapManager.Models;
using HellMapManager.Services;
using System.Threading.Tasks;
namespace HellMapManager.States;


public partial class AppState()
{
    public static readonly AppState Main = new();
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
    private void AddRecent(RecentFile recent)
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
            Current = new MapFile()
            {
                Map = mf,
                Modified = false,
                Path = file,
            };
            AddRecent(Current.ToRecentFile());
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void SaveFile(string file)
    {
        if (Current != null)
        {
            Current.Map.Arrange();
            HMMFile.Save(file, Current);
            Current.Modified = false;
            Current.Path = file;
            AddRecent(Current.ToRecentFile());
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void Exit()
    {
        RaiseExitEvent(this);
    }

    public void NewMap()
    {
        var mapfile = MapFile.Empty("", "");
        SetCurrent(mapfile);
    }
    public void SetCurrent(MapFile mapfile)
    {
        Current = mapfile;
        RaiseMapFileUpdatedEvent(this);
    }
    public void CloseCurrent()
    {
        Current = null;
        RaiseMapFileUpdatedEvent(this);
    }
    public void UpdateMapSettings(MapSettings s)
    {
        if (Current != null)
        {
            Current.Map.Encoding = s.Encoding;
            Current.Map.Info.Name = s.Name;
            Current.Map.Info.Desc = s.Desc;
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }

}