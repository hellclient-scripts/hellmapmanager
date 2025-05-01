using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Models;
namespace HellMapManager.Windows.PickRoomWindow;

public partial class PickRoomWindow : Window
{
    public PickRoomWindow()
    {
        InitializeComponent();
    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is PickRoomWindowViewModel vm)
        {
            vm.FilterRooms();
        }
    }
    public void OnPick(object? sender, RoutedEventArgs args)
    {
        if (sender is Button button && button.DataContext is Room room)
        {
            Close(room.Key);
        }
    }

}