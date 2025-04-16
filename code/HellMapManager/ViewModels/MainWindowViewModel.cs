using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
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
    }
    public partial void InitOverview();
    public partial void InitRooms();
    public string Greeting { get; } = "您还没有打开地图文件。";
    public async void OnOpen()
    {
        if (await AppState.Main.ConfirmModified())
        {
            await AppState.Main.OpenFile();
        }
    }
    public async void OnNew()
    {
        if (await AppState.Main.ConfirmModified())
        {
            AppState.Main.NewMap();
        }

    }
    public async void OnImportRoomsH()
    {
        if (await AppState.Main.ConfirmImport())
        {
            await AppState.Main.ImportRoomsH();
        }
    }
    public void OnExit()
    {
        AppState.Main.Exit();
    }
    public ObservableCollection<RecentFile> Recents { get => new ObservableCollection<RecentFile>(AppState.Main.Settings.Recents.ToArray()); }
    public async void OnOpenRecent(String file)
    {
        if (await AppState.Main.ConfirmModified())
        {
            AppState.Main.OpenRecent(file);
        }
    }
    public void OnSave()
    {
        if (AppState.Main.Current is not null)
        {
            if (AppState.Main.Current.Path != "")
            {
                AppState.Main.Save();
            }
            else
            {
                OnSaveAs();
            }
        }
    }
    public bool IsFileModified
    {
        get => AppState.Main.Current != null && AppState.Main.Current.Modified;
    }
    public async void OnSaveAs()
    {
        await AppState.Main.SaveAs();
    }
    public async void OnClose()
    {
        if (await AppState.Main.ConfirmModified())
        {
            AppState.Main.CloseCurrent();
        }
    }
    public async void OnRevert()
    {
        if (await AppState.Main.ConfirmModified())
        {
        }

    }
    public String TitleInfo
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
