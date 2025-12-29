using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using ComponentExit = HellMapManager.Views.Components.Exit;
using HellMapManager.Models;
using HellMapManager.Windows.EditRoomWindow;
using System;
using Avalonia.Interactivity;
using HellMapManager.Cores;
using HellMapManager.Services;

namespace HellMapManager.Windows.RelationMapWindow;


public partial class RelationMapWindow : Window
{
    public RelationMapWindow(RelationMapWindowViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        vm.RefreshEvent += Refresh;
        Closing += Dispose;
    }
    public void Refresh(object? sender, EventArgs e)
    {
        var zb = this.Find<ZoomBorder>("ZoomBorder")!;
        zb.ResetMatrix();
    }
    public void Dispose(object? sender, EventArgs e)
    {
        if (DataContext is RelationMapWindowViewModel)
        {
            ((RelationMapWindowViewModel)DataContext).RefreshEvent -= Refresh;
        }
    }
    public void OnRefreshButtonDoubleTapped(object sender, TappedEventArgs args)
    {
        if (DataContext is RelationMapWindowViewModel vm)
        {
            vm.Refresh();
            var zb = this.Find<ZoomBorder>("ZoomBorder")!;
            zb.ResetMatrix();
        }
    }

    public void OnDoubleTapped(object sender, TappedEventArgs args)
    {
        if (sender is Border)
        {
            var bo = (Border)sender;
            if (bo.DataContext is ViewItem vi)
            {
                ((RelationMapWindowViewModel)DataContext!).EnterViewItem(vi);
            }
        }

    }

    public void OnExitDoubleTapped(object sender, TappedEventArgs args)
    {
        if (sender is ComponentExit.Exit s)
        {
            if (s.DataContext is Exit ex)
            {
                ((RelationMapWindowViewModel)DataContext!).EnterRoomKey(ex.To);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (DataContext is RelationMapWindowViewModel vm)
        {
            var editRoomWindow = new EditRoomWindow.EditRoomWindow()
            {
                DataContext = new EditRoomWindowViewModel(vm.Current.Item.Room, false)
            };
            var result = await editRoomWindow.ShowDialog<Room?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppKernel.MapDatabase.APIRemoveRooms([vm.Current.Item.Room.Key]);
                AppKernel.MapDatabase.APIInsertRooms([result]);
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                vm.EnterUpdatedRoom(result.Key);
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (DataContext is RelationMapWindowViewModel vm)
        {
            if (await AppUI.Confirm("删除", "确定要删除该房间吗？") == false) return;
            AppKernel.MapDatabase.APIRemoveRooms([vm.Current.Item.Room.Key]);
            AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            vm.HistoryBack();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is RelationMapWindowViewModel vm)
        {
            var editRoomWindow = new EditRoomWindow.EditRoomWindow()
            {
                DataContext = new EditRoomWindowViewModel(null, false)
            };
            var result = await editRoomWindow.ShowDialog<Room?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppKernel.MapDatabase.APIInsertRooms([result]);
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                vm.EnterRoomKey(result.Key);
            }
        }
    }

}