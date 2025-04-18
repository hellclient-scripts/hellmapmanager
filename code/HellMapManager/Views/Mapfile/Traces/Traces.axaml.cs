using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditTraceWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;


namespace HellMapManager.Views.Mapfile.Traces;

public partial class Traces : UserControl
{
    public Traces()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterTraces();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditTraceWindow()
            {
                DataContext = new EditTraceWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertTrace(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditTraceWindow()
                {
                    DataContext = new EditTraceWindowViewModel(alias, false)
                };
                var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateTrace(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditTraceWindow()
                {
                    DataContext = new EditTraceWindowViewModel(alias, true)
                };
                var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateTrace(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace alias)
        {
            if (await AppUI.Confirm("删除", "确定要删除该房间吗？") == false) return;
            AppState.Main.RemoveTrace(alias.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}