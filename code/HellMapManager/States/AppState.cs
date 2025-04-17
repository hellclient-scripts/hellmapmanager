using HellMapManager.Models;
using HellMapManager.Services;
using HellMapManager.Interfaces;
using System.Threading.Tasks;
namespace HellMapManager.States;


public partial class AppState(IAppUI ui)
{
    public static readonly AppState Main = new(new DummyAppUI());
    public IAppUI UI = ui;
    public MapFile? Current;
    public Settings Settings = new();

    public async Task OpenFile()
    {
        var file = await UI.AskLoadFile();
        if (file != "")
        {
            LoadFile(file);
        }
    }
    public void OpenRecent(string file)
    {
        if (file != "")
        {
            LoadFile(file);
        }
    }

    public async Task SaveAs()
    {
        if (Current != null)
        {
            var file = await UI.AskSaveAs();
            if (file != "")
            {
                SaveFile(file);
            }
        }
    }
    public void Save()
    {
        if (Current != null)
        {
            SaveFile(Current.Path);
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
    private void LoadFile(string file)
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
            Current.RebuldCache();
            AddRecent(Current.ToRecentFile());
            RaiseMapFileUpdatedEvent(this);
        }
    }
    private void SaveFile(string file)
    {
        if (Current != null)
        {
            Current.Map.Sort();
            HMMFile.Save(file, Current);
            Current.Modified = false;
            Current.Path = file;
            AddRecent(Current.ToRecentFile());
        }
    }
    private void ImportRoomsHFile(string file)
    {
        if (Current != null)
        {
            Current.ImportRooms(RoomsH.Open(file));
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void InsertRoom(Room room)
    {
        if (Current != null)
        {

            Current.InsertRoom(room);
            Current.Map.Sort();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void RemoveRoom(string key)
    {
        if (Current != null)
        {

            Current.RemoveRoom(key);
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public void UpdateRoom(Room old, Room current)
    {
        if (Current != null)
        {

            if (old.Key != current.Key)
            {
                Current.RemoveRoom(old.Key);
            }
            Current.InsertRoom(current);
            Current.Map.Sort();
            Current.MarkAsModified();
            RaiseMapFileUpdatedEvent(this);
        }
    }
    public async Task Open()
    {
        var file = await UI.AskLoadFile();
        if (file != "")
        {
            LoadFile(file);
        }

    }
    public async Task ImportRoomsH()
    {
        var file = await UI.AskImportRoomsH();
        if (file != "")
        {
            ImportRoomsHFile(file);
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
    public async Task<bool> ConfirmModified()
    {
        if (Current == null || !Current.Modified)
        {
            return true;
        }
        return await UI.ConfirmModified();
    }
    public async Task<bool> ConfirmImport()
    {
        if (Current == null || !Current.Modified)
        {
            return true;
        }
        return await UI.ConfirmImport();
    }
    public void UpdateSettings(MapSettings s)
    {
        if (Current != null)
        {
            Current.Map.Encoding = s.Encoding;
            Current.Map.Info.Name = s.Name;
            Current.Map.Info.Desc = s.Desc;
            Current.MarkAsModified();
            RaiseSettingsUpdatedEvent(this);
        }
    }

}