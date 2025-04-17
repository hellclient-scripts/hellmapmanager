using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Windows.EditRoomWindow;

public partial class EditRoomWindow : Window
{
    public EditRoomWindow()
    {
        InitializeComponent();
    }
    public void OnSave(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            var err = vm.Item.Validate();
            if (err != "")
            {
                DialogHelper.Alert("验证失败", err);
                return;
            }
            Close(vm.Item.ToRoom());
        }
    }
    public void OnClose(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm)
        {
            Close(null);
        }
    }
    public void OnEnterEditing(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm && vm.Editable)
        {
            vm.EnterEdit();
        }
    }
    public void OnCancel(object? sender, RoutedEventArgs args)
    {
        if (DataContext is EditRoomWindowViewModel vm && vm.Editing)
        {
            vm.CancelEdit();
        }
    }
}