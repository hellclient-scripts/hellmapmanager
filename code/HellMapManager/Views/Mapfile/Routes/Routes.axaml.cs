using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditRouteWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;


namespace HellMapManager.Views.Mapfile.Routes;

public partial class Routes : UserControl
{
    public Routes()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterRoutes();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditRouteWindow()
            {
                DataContext = new EditRouteWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Route?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertRoute(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Route alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditRouteWindow()
                {
                    DataContext = new EditRouteWindowViewModel(alias, false)
                };
                var result = await window.ShowDialog<Route?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRoute(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Route alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditRouteWindow()
                {
                    DataContext = new EditRouteWindowViewModel(alias, true)
                };
                var result = await window.ShowDialog<Route?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRoute(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Route model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该路线吗？") == false) return;
            AppState.Main.RemoveRoute(model.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}