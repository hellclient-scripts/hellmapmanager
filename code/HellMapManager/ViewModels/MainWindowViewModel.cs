using System;
using HellMapManager.States;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using HellMapManager.Windows.NewFileDialog;
using HellMapManager.Services;
namespace HellMapManager.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    [SetsRequiredMembersAttribute]
    public MainWindowViewModel(AppState state)
    {
        AppState = state;
        AppState.NewFileDialogEvent += OpenNewFileDialog;
    }
    public void OpenNewFileDialog(object? sender)
    {
        Console.WriteLine("OpenNewFileDialog");
        var dialog = new NewFileDialog();
        dialog.ShowDialog(AppState.Desktop.MainWindow!);
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
