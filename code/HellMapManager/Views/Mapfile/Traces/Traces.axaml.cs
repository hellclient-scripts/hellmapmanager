using Avalonia.Controls;
using HellMapManager.ViewModels;
using HellMapManager.Models;
using HellMapManager.Windows.EditTraceWindow;
using Avalonia.Interactivity;
using HellMapManager.Cores;
using HellMapManager.Services;


namespace HellMapManager.Views.Mapfile.Traces;

public partial class Traces : UserControl
{
    public Traces()
    {
        InitializeComponent();

    }
    public void OnFilter(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.FilterTraces();
        }
    }
    public async void OnNew(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            var window = new EditTraceWindow()
            {
                DataContext = new EditTraceWindowViewModel(null, false)
            };


            var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
            if (result is not null)
            {
                AppKernel.MapDatabase.APIInsertTraces([result]);
                AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
            }
        }
    }
    public async void OnEdit(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditTraceWindow()
                {
                    DataContext = new EditTraceWindowViewModel(model, false)
                };
                var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveTraces([model.Key]);
                    AppKernel.MapDatabase.APIInsertTraces([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);

                }
            }
        }
    }
    public async void OnView(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace model)
        {
            if (Parent is not null && Parent.DataContext is MainWindowViewModel vm)
            {
                var window = new EditTraceWindow()
                {
                    DataContext = new EditTraceWindowViewModel(model, true)
                };
                var result = await window.ShowDialog<Trace?>((TopLevel.GetTopLevel(this) as Window)!);
                if (result is not null)
                {
                    AppKernel.MapDatabase.APIRemoveTraces([model.Key]);
                    AppKernel.MapDatabase.APIInsertTraces([result]);
                    AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
                }
            }
        }
    }
    public async void OnRemove(object? sender, RoutedEventArgs args)
    {
        if (sender is not null && sender is Button bn && bn.DataContext is Trace model)
        {
            if (await AppUI.Confirm("删除", "确定要删除该足迹吗？") == false) return;
            AppKernel.MapDatabase.APIRemoveTraces([model.Key]);
            AppKernel.MapDatabase.RaiseMapFileUpdatedEvent(this);
        }
    }
}