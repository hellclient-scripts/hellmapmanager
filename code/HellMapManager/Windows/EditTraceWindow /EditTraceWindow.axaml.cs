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
namespace HellMapManager.Windows.EditTraceWindow;

public partial class EditTraceWindow : Window
{
    public EditTraceWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditTraceWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                AppUI.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToTrace());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditTraceWindowViewModel vm)
        {
            Close(null);
        }
    }

    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditTraceWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditTraceWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}