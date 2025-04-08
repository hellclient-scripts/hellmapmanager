using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.PanAndZoom;
using Avalonia.Markup.Xaml;
using HellMapManager.Services;
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
}