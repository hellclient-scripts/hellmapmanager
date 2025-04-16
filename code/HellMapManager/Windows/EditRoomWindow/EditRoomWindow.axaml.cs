using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;

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
            Close(vm.Item);
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