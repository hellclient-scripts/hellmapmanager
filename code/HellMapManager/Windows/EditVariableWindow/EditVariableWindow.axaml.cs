using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
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