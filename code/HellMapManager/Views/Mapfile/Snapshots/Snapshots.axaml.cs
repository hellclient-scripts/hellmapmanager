using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditSnapshotWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Snapshots;

public partial class Snapshots : UserControl
{
    public Snapshots()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterSnapshots();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditSnapshotWindow()
            {
                DataContext = new EditSnapshotWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Snapshot?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.APIInsertSnapshots([result]);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Snapshot model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditSnapshotWindow()
                {
                    DataContext = new EditSnapshotWindowViewModel(model, true)
                };
                await window.ShowDialog<Snapshot?>((TopLevel.GetTopLevel(this) as Window)!);
                // if (result is not null)
                // {
                //     AppState.Main.UpdateSnapshot(model, result);
                //     AppState.Main.RaiseMapFileUpdatedEvent(this);
                // }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Snapshot model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该快照吗？") == false) return;
            AppState.Main.APIRemoveSnapshots([model.UniqueKey()]);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}