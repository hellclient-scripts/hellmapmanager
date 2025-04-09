using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using HellMapManager.Services;
using ComponentExit = HellMapManager.Views.Components.Exit;
using HellMapManager.Models;
using System;

namespace HellMapManager.Windows.RelationMapWindow;


public partial class RelationMapWindow : Window
{
    public RelationMapWindow(RelationMapWindowViewModel vm)
    {
        InitializeComponent();
        this.DataContext = vm;
        vm.RefreshEvent += this.Refresh;
        this.Closing += Dispose;
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
    public void OnDoubleTapped(object sender, TappedEventArgs args)
    {
        if (sender is Border)
        {
            var bo = (Border)sender;
            if (bo.DataContext is ViewItem)
            {
                var vi = (ViewItem)bo.DataContext;
                ((RelationMapWindowViewModel)DataContext!).EnterViewItem(vi);
            }
        }

    }

    public void OnExitDoubleTapped(object sender, TappedEventArgs args)
    {
        if (sender is ComponentExit.Exit)
        {
            var s = (ComponentExit.Exit)sender;
            if (s.DataContext is Exit)
            {
                var ex = (Exit)s.DataContext;
                var p = this.Find<Panel>("RoomDetail")!;
                ((RelationMapWindowViewModel)DataContext!).EnterRoomKey(ex.To);
            }
        }
    }

}