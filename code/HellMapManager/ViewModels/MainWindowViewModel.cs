using System;
using HellMapManager.Services;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.MicroCom;

namespace HellMapManager.ViewModels;
public delegate void Action();

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    public async static void OnOpen(object sender)
    {
        Console.WriteLine("Open");
        Console.WriteLine(await FileDialog.LoadFile(sender));
    }
    public async static void OnNew(object sender)
    {
        Console.WriteLine("Open");
        Console.WriteLine(await FileDialog.LoadFile(sender));
    }
    public void OnExit()
    {
        this.ShotdownAction();
    }
    public required Action ShotdownAction;
}
