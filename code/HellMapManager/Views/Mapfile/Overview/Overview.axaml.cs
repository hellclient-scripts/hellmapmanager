using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Models;
using HellMapManager.Cores;
using HellMapManager.ViewModels;
using HellMapManager.Windows.UpdateMapWindow;

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
                    case "WidgetMarker":
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
    public async void OnUpdateButtonClicked(object? sender, RoutedEventArgs args)
    {
        if (DataContext is MainWindowViewModel vm && AppKernel.Instance.MapDatabase.Current is not null)
        {
            var settings = AppKernel.Instance.MapDatabase.Current.ToSettings();
            var nwm = new UpdateMapWindowViewModel(settings);
            var nw = new UpdateMapWindow
            {
                DataContext = nwm
            };
            var result = await nw.ShowDialog<MapSettings>((TopLevel.GetTopLevel(this) as Window)!);
            if (result != null)
            {
                AppKernel.Instance.MapDatabase.UpdateMapSettings(result);
            }
        }
    }
}