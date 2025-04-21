using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    public MainWindowViewModel()
    {
        AppState.Main.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(Recents));
            OnPropertyChanged(nameof(IsFileModified));
            OnPropertyChanged(nameof(TitleInfo));
            OnPropertyChanged(nameof(CanShowWelcome));
            OnPropertyChanged(nameof(IsFileOpend));
        };
        InitOverview();
        InitRooms();
        InitAliases();
        InitRoutes();
        InitTraces();
        InitRegions();
        InitLandmarks();
        InitShortcuts();
    }

    public partial void InitOverview();
    public partial void InitRooms();
    public partial void InitAliases();
    public partial void InitRoutes();
    public partial void InitTraces();
    public partial void InitRegions();
    public partial void InitLandmarks();
    public partial void InitShortcuts();
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
            AppState.Main.NewMap();
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
        AppState.Main.Exit();
    }
    public ObservableCollection<RecentFile> Recents { get => new ObservableCollection<RecentFile>(AppState.Main.Settings.Recents.ToArray()); }
    public async void OnOpenRecent(String file)
    {
        await AppUI.Main.OnOpenRecent(file);
    }
    public bool IsFileModified
    {
        get => AppState.Main.Current != null && AppState.Main.Current.Modified;
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
            AppState.Main.CloseCurrent();
        }
    }
    public void OnRevert()
    {
        AppUI.Main.Revert();
    }
    public string TitleInfo
    {
        get => (AppState.Main.Current == null ? "" : (AppState.Main.Current.Modified ? "* " : "") + (AppState.Main.Current.Path != "" ? AppState.Main.Current.Path : "<未保存>") + " ") + "Hell Map Manager";
    }
    public bool CanShowWelcome
    {
        get => AppState.Main.Current == null;
    }
    public bool IsFileOpend
    {
        get => AppState.Main.Current != null;
    }
}
