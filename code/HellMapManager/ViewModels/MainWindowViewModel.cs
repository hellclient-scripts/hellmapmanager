using System;
using HellMapManager.States;
using System.Collections.ObjectModel;

namespace HellMapManager.ViewModels;
public delegate void Action();

public partial class MainWindowViewModel : ViewModelBase
{
    public required AppState AppState;
    public string Greeting { get; } = "Welcome to Avalonia!";
    public async void OnOpen()
    {
        Console.WriteLine("Open");
        Console.WriteLine(await this.AppState.OpenFile());
    }
    public async void OnNew()
    {
        Console.WriteLine("Open");
        Console.WriteLine(await this.AppState.OpenFile());
    }
    public void OnExit()
    {
        this.AppState.Exit();
    }
    public ObservableCollection<String> Recents { get => new ObservableCollection<String>(this.AppState.Recents.ToArray()); }
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
