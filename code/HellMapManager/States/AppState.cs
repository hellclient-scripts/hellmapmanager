using HellMapManager.Models;
using HellMapManager.Services;
using HellMapManager.Interfaces;
using System.Threading.Tasks;
using System;
namespace HellMapManager.States;


public partial class AppState(IAppUI ui)
{
    public IAppUI UI = ui;
    public MapFile? Current;
    public Settings Settings = new Settings();

    public async Task OpenFile()
    {
        var file = await UI.AskLoadFile();
        if (file != "")
        {
            this.LoadFile(file);
        }
    }
    public void OpenRecent(string file)
    {
        if (file != "")
        {
            this.LoadFile(file);
        }
    }

    public async Task SaveAs()
    {
        if (this.Current != null)
        {
            var file = await UI.AskSaveAs();
            if (file != "")
            {
                this.SaveFile(file);
            }
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
            this.AddRecent(Current.ToRecentFile());
            this.RaiseMapFileUpdatedEvent(this);
        }
    }
    private void SaveFile(string file)
    {
        if (this.Current != null)
        {
            this.Current.Map.Sort();
            HMMFile.Save(file, this.Current);
            this.Current.Modified = false;
            this.Current.Path = file;
            this.AddRecent(Current.ToRecentFile());
            this.RaiseMapFileUpdatedEvent(this);
        }
    }
    private void ImportRoomsHFile(string file)
    {
        if (this.Current != null)
        {
            this.Current.ImportRooms(RoomsH.Open(file));
            this.RaiseMapFileUpdatedEvent(this);
        }
    }

    public async Task Open()
    {
        var file = await UI.AskLoadFile();
        if (file != "")
        {
            this.LoadFile(file);
        }

    }
    public async Task ImportRoomsH()
    {
        var file = await UI.AskImportRoomsH();
        if (file != "")
        {
            this.ImportRoomsHFile(file);
        }

    }

    public void Exit()
    {
        RaiseExitEvent(this);
    }

    public void NewMap()
    {
        var mapfile = MapFile.Empty("", "");
        this.SetCurrent(mapfile);
    }
    public void SetCurrent(MapFile mapfile)
    {
        this.Current = mapfile;
        this.RaiseMapFileUpdatedEvent(this);
    }
    public void CloseCurrent()
    {
        this.Current = null;
        this.RaiseMapFileUpdatedEvent(this);
    }
    public async Task<bool> ConfirmModified()
    {
        if (this.Current == null || !this.Current.Modified)
        {
            return true;
        }
        return await UI.ConfirmModified();
    }
    public async Task<bool> ConfirmImport()
    {
        if (this.Current == null || !this.Current.Modified)
        {
            return true;
        }
        return await UI.ConfirmImport();
    }


}