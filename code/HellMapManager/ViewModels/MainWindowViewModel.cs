using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HellMapManager.ViewModels;
public delegate void Action();

public partial class MainWindowViewModel : ViewModelBase
{
    public required AppState AppState;
    public string Greeting { get; } = "您还没有打开地图文件。";
    public async void OnOpen()
    {
        Console.WriteLine("Open");
        Console.WriteLine(await this.AppState.OpenFile());
    }
    public async void OnNew()
    {
        Console.WriteLine("New");
        Console.WriteLine(await this.AppState.OpenFile());
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
        get => this.AppState.Current != null && this.AppState.Current.Modfied;
    }
    public void OnSaveAs()
    {
    }
    public bool CanSaveAs
    {
        get => this.AppState.Current != null;
    }
}
