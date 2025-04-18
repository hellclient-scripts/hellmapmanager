using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditRoomWindow;
using Avalonia.Interactivity;
using HellMapManager.States;

namespace HellMapManager.Views.Mapfile.Rooms;

public partial class Rooms : UserControl
{
    public Rooms()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterRooms();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var editRoomWindow = new EditRoomWindow()
            {
                DataContext = new EditRoomWindowViewModel(null, false)
            };


            var result = await editRoomWindow.ShowDialog<Room?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppState.Main.InsertRoom(result);
                AppState.Main.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Room room)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var editRoomWindow = new EditRoomWindow()
                {
                    DataContext = new EditRoomWindowViewModel(room, false)
                };
                var result = await editRoomWindow.ShowDialog<Room?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRoom(room, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Room room)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var editRoomWindow = new EditRoomWindow()
                {
                    DataContext = new EditRoomWindowViewModel(room, true)
                };
                var result = await editRoomWindow.ShowDialog<Room?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppState.Main.UpdateRoom(room, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }

}