using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using HellMapManager.Models;
using System.Threading.Tasks;
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
            await this.AppState.OpenFile();
        }
    }
    public async Task OnNew()
    {
        if (await AppState.ConfirmModified())
        {
            Console.WriteLine("创建了地图文件");
            AppState.NewMap();
        }

    }
    public async void OnImportRoomsH()
    {
        if (await AppState.ConfirmImport())
        {
            await this.AppState.ImportRoomsH();
        }
    }
    public void OnExit()
    {
        this.AppState.Exit();
    }
    public ObservableCollection<RecentFile> Recents { get => new ObservableCollection<RecentFile>(this.AppState.Settings.Recents.ToArray()); }
    public async void OnOpenRecent(String file)
    {
        if (await AppState.ConfirmModified())
        {
            AppState.OpenRecent(file);
        }
    }
    public void OnSave()
    {
    }
    public bool IsFileModified
    {
        get => this.AppState.Current != null && this.AppState.Current.Modified;
    }
    public async void OnSaveAs()
    {
        await AppState.SaveAs();
    }
    public async void OnClose()
    {
        if (await AppState.ConfirmModified())
        {
            this.AppState.CloseCurrent();
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
        get => (AppState.Current == null ? "" : (AppState.Current.Modified ? "* " : "") + (AppState.Current.Path != "" ? AppState.Current.Path : "<未保存>") + " ") + "HellMapManager";
    }
    public bool CanShowWelcome
    {
        get => this.AppState.Current == null;
    }
    public bool IsFileOpend
    {
        get => this.AppState.Current != null;
    }
}
