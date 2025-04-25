using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditRegionWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;


namespace HellMapManager.Views.Mapfile.Regions;

public partial class Regions : UserControl
{
    public Regions()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterRegions();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditRegionWindow()
            {
                DataContext = new EditRegionWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Region?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertRegion(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Region alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditRegionWindow()
                {
                    DataContext = new EditRegionWindowViewModel(alias, false)
                };
                var result = await window.ShowDialog<Region?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRegion(alias.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Region alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditRegionWindow()
                {
                    DataContext = new EditRegionWindowViewModel(alias, true)
                };
                var result = await window.ShowDialog<Region?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRegion(alias.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Region alias)
        {
            if (await AppUI.Confirm("删除", "确定要删除该地区吗？") == false) return;
            AppState.Main.RemoveRegion(alias.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}