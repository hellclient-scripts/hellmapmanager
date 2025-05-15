using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditMarkerWindow;
using Avalonia.Interactivity;
using HellMapManager.Cores;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Markers;

public partial class Markers : UserControl
{
    public Markers()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterMarkers();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditMarkerWindow()
            {
                DataContext = new EditMarkerWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Marker?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppKernel.MapDatabase.APIInsertMarkers([result]);
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Marker marker)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditMarkerWindow()
                {
                    DataContext = new EditMarkerWindowViewModel(marker, false)
                };
                var result = await window.ShowDialog<Marker?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveMarkers([marker.Key]);
                    AppKernel.MapDatabase.APIInsertMarkers([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Marker marker)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditMarkerWindow()
                {
                    DataContext = new EditMarkerWindowViewModel(marker, true)
                };
                var result = await window.ShowDialog<Marker?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveMarkers([marker.Key]);
                    AppKernel.MapDatabase.APIInsertMarkers([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Marker marker)
        {
            if (await AppUI.Confirm("删除", "确定要删除该房间吗？") == false) return;
            AppKernel.MapDatabase.APIRemoveMarkers([marker.Key]);
            AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
        }
    }
}