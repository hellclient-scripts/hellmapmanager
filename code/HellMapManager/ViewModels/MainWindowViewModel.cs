using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    public MainWindowViewModel(AppState state)
    {
        AppState = state;
        AppState.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
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
    public AppState AppState;
    public string Greeting { get; } = "您还没有打开地图文件。";
    public async void OnOpen()
    {
        if (await AppState.ConfirmModified())
        {
            await AppState.OpenFile();
        }
    }
    public async void OnNew()
    {
        if (await AppState.ConfirmModified())
        {
            AppState.NewMap();
        }

    }
    public async void OnImportRoomsH()
    {
        if (await AppState.ConfirmImport())
        {
            await AppState.ImportRoomsH();
        }
    }
    public void OnExit()
    {
        AppState.Exit();
    }
    public ObservableCollection<RecentFile> Recents { get => new ObservableCollection<RecentFile>(AppState.Settings.Recents.ToArray()); }
    public async void OnOpenRecent(String file)
    {
        if (await AppState.ConfirmModified())
        {
            AppState.OpenRecent(file);
        }
    }
    public void OnSave()
    {
        if (AppState.Current is not null)
        {
            if (AppState.Current.Path != "")
            {

            }
            else
            {
                OnSaveAs();
            }
        }
    }
    public bool IsFileModified
    {
        get => AppState.Current != null && AppState.Current.Modified;
    }
    public async void OnSaveAs()
    {
        await AppState.SaveAs();
    }
    public async void OnClose()
    {
        if (await AppState.ConfirmModified())
        {
            AppState.CloseCurrent();
        }
    }
    public async void OnRevert()
    {
        if (await AppState.ConfirmModified())
        {
        }

    }
    public String TitleInfo
    {
        get => (AppState.Current == null ? "" : (AppState.Current.Modified ? "* " : "") + (AppState.Current.Path != "" ? AppState.Current.Path : "<未保存>") + " ") + "Hell Map Manager";
    }
    public bool CanShowWelcome
    {
        get => AppState.Current == null;
    }
    public bool IsFileOpend
    {
        get => AppState.Current != null;
    }
}
