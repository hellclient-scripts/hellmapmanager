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

    public async Task<String> OpenFile()
    {
        return await DialogManager.LoadFile(this.Desktop.MainWindow!);
    }
    public void SaveFile()
    {

    }
    public void SaveAsFile()
    {

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
}