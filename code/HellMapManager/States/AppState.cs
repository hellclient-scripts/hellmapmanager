using HellMapManager.Models;
using HellMapManager.Services;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System;
namespace HellMapManager.States;


public partial class AppState
{
    public MapFile? Current;
    public Settings Settings = new Settings();
    public required IClassicDesktopStyleApplicationLifetime Desktop;

    public async Task OpenFile()
    {
        var file = await DialogManager.LoadFile(this.Desktop.MainWindow!);
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
            var file = await DialogManager.SaveAs(this.Desktop.MainWindow!);
            if (file != "")
            {
                this.SaveFile(file);
            }
        }
    }
    private void AddRecent(RecentFile recent)
    {
        this.Settings.Recents.RemoveAll(r => r.Path == recent.Path);
        this.Settings.Recents.Insert(0, recent);
        if (this.Settings.Recents.Count > AppPreset.MaxRecents)
        {
            this.Settings.Recents = this.Settings.Recents.GetRange(0, AppPreset.MaxRecents);
        }
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
            this.Current.MarkAsModified();
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
        var file = await DialogManager.LoadFile(this.Desktop.MainWindow!);
        if (file != "")
        {
            this.LoadFile(file);
        }

    }
    public async Task ImportRoomsH()
    {
        var file = await DialogManager.ImportRoomsH(this.Desktop.MainWindow!);
        if (file != "")
        {
            this.ImportRoomsHFile(file);
        }

    }

    public void Exit()
    {
        this.Desktop.Shutdown(0);
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
        return await DialogManager.ConfirmModifiedDialog();
    }
    public async Task<bool> ConfirmImport()
    {
        if (this.Current == null || !this.Current.Modified)
        {
            return true;
        }
        return await DialogManager.ConfirmImportDialog();
    }


}