using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Input;
using ComponentExit = HellMapManager.Views.Components.Exit;
using HellMapManager.Models;
using System;

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
        var zb = this.Find<ZoomBorder>("ZoomBorder")!;
        zb.ResetMatrix();
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

}