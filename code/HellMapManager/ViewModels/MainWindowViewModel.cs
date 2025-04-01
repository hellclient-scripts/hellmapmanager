using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Models;
using HellMapManager.Services;
using System.Threading.Tasks;
using System.Reflection;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    [SetsRequiredMembersAttribute]
    public MainWindowViewModel(AppState state)
    {
        AppState = state;
        AppState.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(IsFileModified));
            OnPropertyChanged(nameof(TitleInfo));
            OnPropertyChanged(nameof(CanShowWelcome));
            OnPropertyChanged(nameof(IsFileOpend));
            OnPropertyChanged(nameof(GetMap));
        };
    }
    public required AppState AppState;
    public string Greeting { get; } = "您还没有打开地图文件。";
    public async void OnOpen()
    {
        Console.WriteLine("Open");
        Console.WriteLine(await this.AppState.OpenFile());
    }
    public void OnNew()
    {
        Console.WriteLine("创建了地图文件");
        AppState.NewMap();
    }
    public void OnExit()
    {
        this.AppState.Exit();
    }
    public ObservableCollection<String> Recents { get => new ObservableCollection<String>(this.AppState.Settings.Recents.ToArray()); }
    public void OnOpenRecent(String name)
    {
        Console.WriteLine(name);
    }
    public void OnSave()
    {
    }
    public bool IsFileModified
    {
        get => this.AppState.Current != null && this.AppState.Current.Modified;
    }
    public void OnSaveAs()
    {
    }
    public void OnClose()
    {
        this.AppState.CloseCurrent();
    }
    public void OnRevert()
    {

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
    public MapFile? GetMap
    {
        get => AppState.Current;
    }
}
