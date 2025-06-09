using System;
using HellMapManager.Cores;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    public MainWindowViewModel()
    {
        AppKernel.MapDatabase.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(Recents));
            OnPropertyChanged(nameof(IsFileModified));
            OnPropertyChanged(nameof(TitleInfo));
            OnPropertyChanged(nameof(CanShowWelcome));
            OnPropertyChanged(nameof(IsFileOpend));
        };
        InitOverview();
        InitRooms();
        InitMarkers();
        InitRoutes();
        InitTraces();
        InitRegions();
        InitLandmarks();
        InitShortcuts();
        InitVariables();
        InitSnapshots();
    }

    public partial void InitOverview();
    public partial void InitRooms();
    public partial void InitMarkers();
    public partial void InitRoutes();
    public partial void InitTraces();
    public partial void InitRegions();
    public partial void InitLandmarks();
    public partial void InitShortcuts();
    public partial void InitVariables();
    public partial void InitSnapshots();

    public string Greeting { get; } = "您还没有打开地图文件。";
    public async void OnOpen()
    {
        if (await AppUI.Main.ConfirmModified())
        {
            await AppUI.Main.OpenFile();
        }
    }
    public async void OnNew()
    {
        if (await AppUI.Main.ConfirmModified())
        {
            AppKernel.MapDatabase.NewMap();
        }

    }
    public async void OnImportRoomsH()
    {
        if (await AppUI.Main.ConfirmImport())
        {
            await AppUI.Main.ImportRoomsH();
        }
    }
    public void OnExit()
    {
        AppKernel.MapDatabase.Exit();
    }
    public ObservableCollection<RecentFile> Recents { get => new ObservableCollection<RecentFile>(AppKernel.MapDatabase.Settings.Recents.ToArray()); }
    public async void OnOpenRecent(String file)
    {
        await AppUI.Main.OnOpenRecent(file);
    }
    public bool IsFileModified
    {
        get => AppKernel.MapDatabase.Current != null && AppKernel.MapDatabase.Current.Modified;
    }
    public async void OnSave()
    {
        await AppUI.Main.Save();
    }
    public async void OnSaveAs()
    {
        await AppUI.Main.SaveAs();
    }
    public async void OnClose()
    {
        if (await AppUI.Main.ConfirmModified())
        {
            AppKernel.MapDatabase.CloseCurrent();
        }
    }
    public void OnRevert()
    {
        AppUI.Main.Revert();
    }
    public string TitleInfo
    {
        get => (AppKernel.MapDatabase.Current == null ? "" : (AppKernel.MapDatabase.Current.Modified ? "* " : "") + (AppKernel.MapDatabase.Current.Path != "" ? AppKernel.MapDatabase.Current.Path : "<未保存>") + " ") + "HellMapManager";
    }
    public bool CanShowWelcome
    {
        get => AppKernel.MapDatabase.Current == null;
    }
    public bool IsFileOpend
    {
        get => AppKernel.MapDatabase.Current != null;
    }
}
