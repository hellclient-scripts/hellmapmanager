using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Windows.NewFileDialog;
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
        AppState.NewFileDialogEvent += OpenNewFileDialog;
        AppState.MapFileUpdatedEvent += (object? sender, EventArgs args) =>
        {
            OnPropertyChanged(nameof(CanSave));
            OnPropertyChanged(nameof(CanSaveAs));
            OnPropertyChanged(nameof(TitleInfo));
            OnPropertyChanged(nameof(CanClose));
            OnPropertyChanged(nameof(CanShowWelcome));
            OnPropertyChanged(nameof(CanShowMapFile));
            OnPropertyChanged(nameof(GetMap));
        };
    }
    public async void OpenNewFileDialog(object? sender, EventArgs args)
    {
        Console.WriteLine("OpenNewFileDialog");
        var dialog = new NewFileDialog();
        var mudfile = await dialog.ShowDialog<MapFile?>(AppState.Desktop.MainWindow!);
        if (mudfile != null)
        {
            Console.WriteLine("创建了地图文件");
            AppState.SetCurrent(mudfile);
        }
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
        AppState.RaiseNewFileDialogEvent(this);
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
    public bool CanSave
    {
        get => this.AppState.Current != null && this.AppState.Current.Modified;
    }
    public void OnSaveAs()
    {
    }
    public bool CanSaveAs
    {
        get => this.AppState.Current != null;
    }
    public bool CanClose
    {
        get => this.AppState.Current != null;
    }
    public void OnClose()
    {
        this.AppState.CloseCurrent();
    }
    public String TitleInfo
    {
        get => (AppState.Current == null ? "" : (AppState.Current.Modified ? "* " : "") + (AppState.Current.Path != "" ? AppState.Current.Path : "<未保存>") + " ") + "HellMapManager";
    }
    public bool CanShowWelcome
    {
        get => this.AppState.Current == null;
    }
    public bool CanShowMapFile
    {
        get => this.AppState.Current != null;
    }
    public MapFile? GetMap
    {
        get => AppState.Current;
    }
}
