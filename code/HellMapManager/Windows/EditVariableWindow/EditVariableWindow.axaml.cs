using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Services;
using HellMapManager.Models;
using HellMapManager.Windows.EditDataWindow;
using HellMapManager.Windows.EditExitWindow;
using HellMapManager.Windows.NewTagWindow;
using System;
namespace HellMapManager.Windows.EditVariableWindow;

public partial class EditVariableWindow : Window
{
    public EditVariableWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditVariableWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToVariable());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditVariableWindowViewModel vm)
        {
            Close(null);
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditVariableWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditVariableWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}