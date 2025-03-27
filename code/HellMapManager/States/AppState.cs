using HellMapManager.Models;
using HellMapManager.Services;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
namespace HellMapManager.States;


public class AppState
{
    public MapFile? Current;
    public required IClassicDesktopStyleApplicationLifetime Desktop;
    public List<String> Recents = ["111","222","333"];
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