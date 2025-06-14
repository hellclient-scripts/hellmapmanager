using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditShortcutWindow;
using Avalonia.Interactivity;
using HellMapManager.Cores;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Shortcuts;

public partial class Shortcuts : UserControl
{
    public Shortcuts()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterShortcuts();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditShortcutWindow()
            {
                DataContext = new EditShortcutWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Shortcut?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppKernel.MapDatabase.APIInsertShortcuts([result]);
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Shortcut model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditShortcutWindow()
                {
                    DataContext = new EditShortcutWindowViewModel(model, false)
                };
                var result = await window.ShowDialog<Shortcut?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveShortcuts([model.Key]);
                    AppKernel.MapDatabase.APIInsertShortcuts([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Shortcut model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditShortcutWindow()
                {
                    DataContext = new EditShortcutWindowViewModel(model, true)
                };
                var result = await window.ShowDialog<Shortcut?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveShortcuts([model.Key]);
                    AppKernel.MapDatabase.APIInsertShortcuts([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Shortcut model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该捷径吗？") == false) return;
            AppKernel.MapDatabase.APIRemoveShortcuts([model.Key]);
            AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
        }
    }
}