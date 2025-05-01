using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Services;
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