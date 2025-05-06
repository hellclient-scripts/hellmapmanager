using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditRoomWindow;
using Avalonia.Interactivity;
using HellMapManager.States;
using HellMapManager.Services;
using HellMapManager.Windows.RelationMapWindow;


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
                AppState.Main.APIInsertRooms([result]);
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
                    AppState.Main.APIUpdateRoom(room.Key, result);
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
                    AppState.Main.APIUpdateRoom(room.Key, result);
                    AppState.Main.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnDumpRoom(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Room room)
        {
            var rm = RelationMapper.RelationMap(AppState.Main.Current!, room.Key, AppPreset.RelationMaxDepth);
            if (rm is not null)
            {
                var vm = new RelationMapWindowViewModel(rm);
                var Window = new RelationMapWindow(vm);
                await Window.ShowDialog(AppUI.Main.Desktop.MainWindow!);
            }
        }
    }

    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Room room)
        {
            if (await AppUI.Confirm("删除", "确定要删除该房间吗？") == false) return;
            AppState.Main.APIRemoveRoom(room.Key);
            AppState.Main.RaiseMapFileUpdatedEvent(this);
        }
    }
}