using System;
using HellMapManager.States;
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
}
