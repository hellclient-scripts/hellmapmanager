using HellMapManager.Models;
using HellMapManager.Services;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System;
namespace HellMapManager.States;


public class AppState
{
    public MapFile? Current;
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
}