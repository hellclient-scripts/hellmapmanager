using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditVariableWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Variables;

public partial class Variables : UserControl
{
    public Variables()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterVariables();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditVariableWindow()
            {
                DataContext = new EditVariableWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Variable?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.APIInsertVariables([result]);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Variable model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditVariableWindow()
                {
                    DataContext = new EditVariableWindowViewModel(model, false)
                };
                var result = await window.ShowDialog<Variable?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.APIUpdateVariable(model.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Variable model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditVariableWindow()
                {
                    DataContext = new EditVariableWindowViewModel(model, true)
                };
                var result = await window.ShowDialog<Variable?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.APIUpdateVariable(model.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Variable model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该变量吗？") == false) return;
            AppState.Main.APIRemoveVariable(model.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}