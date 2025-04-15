using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace HellMapManager.Views.Mapfile.Overview;

public partial class Overview : UserControl
{
    public Overview()
    {
        InitializeComponent();
    }
    public void OnWidgetClicked(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is Border b)
        {
            var ns = this.Parent!.FindNameScope()!;
            var tab = ns.Find<TabControl>("MainTab")!;
            if (tab != null)
            {
                switch (b.Name)
                {
                    case "WidgetRoom":
                        tab.SelectedIndex = 1;
                        break;
                    case "WidgetAlias":
                        tab.SelectedIndex = 2;
                        break;
                    case "WidgetRoute":
                        tab.SelectedIndex = 3;
                        break;
                    case "WidgetTrace":
                        tab.SelectedIndex = 4;
                        break;
                    case "WidgetRegion":
                        tab.SelectedIndex = 5;
                        break;
                    case "WidgetLandmark":
                        tab.SelectedIndex = 6;
                        break;
                    case "WidgetShortcut":
                        tab.SelectedIndex = 7;
                        break;
                    case "WidgetVariable":
                        tab.SelectedIndex = 8;
                        break;
                    case "WidgetSnapshot":
                        tab.SelectedIndex = 9;
                        break;
                    case "WidgetQuery":
                        tab.SelectedIndex = 10;
                        break;
                }
            }
        }
    }
}