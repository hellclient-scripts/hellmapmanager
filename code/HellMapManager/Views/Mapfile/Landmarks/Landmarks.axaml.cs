using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditLandmarkWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Landmarks;

public partial class Landmarks : UserControl
{
    public Landmarks()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterLandmarks();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditLandmarkWindow()
            {
                DataContext = new EditLandmarkWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Landmark?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertLandmark(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Landmark alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditLandmarkWindow()
                {
                    DataContext = new EditLandmarkWindowViewModel(alias, false)
                };
                var result = await window.ShowDialog<Landmark?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateLandmark(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Landmark alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditLandmarkWindow()
                {
                    DataContext = new EditLandmarkWindowViewModel(alias, true)
                };
                var result = await window.ShowDialog<Landmark?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateLandmark(alias, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Landmark model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该定位吗？") == false) return;
            AppState.Main.RemoveLandmark(model.Key, model.Type);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}