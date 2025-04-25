using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditAliasWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;


namespace HellMapManager.Views.Mapfile.Aliases;

public partial class Aliases : UserControl
{
    public Aliases()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterAliases();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditAliasWindow()
            {
                DataContext = new EditAliasWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Alias?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertAlias(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Alias alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditAliasWindow()
                {
                    DataContext = new EditAliasWindowViewModel(alias, false)
                };
                var result = await window.ShowDialog<Alias?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateAlias(alias.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Alias alias)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditAliasWindow()
                {
                    DataContext = new EditAliasWindowViewModel(alias, true)
                };
                var result = await window.ShowDialog<Alias?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateAlias(alias.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Alias alias)
        {
            if (await AppUI.Confirm("删除", "确定要删除该房间吗？") == false) return;
            AppState.Main.RemoveAlias(alias.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}