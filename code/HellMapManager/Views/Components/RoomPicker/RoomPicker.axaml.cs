using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using HellMapManager.Windows.PickRoomWindow;
using Avalonia.Data;

namespace HellMapManager.Views.Components.RoomPicker;

public partial class RoomPicker : UserControl
{
    public static StyledProperty<string> RoomProperty
    = AvaloniaProperty.Register<RoomPicker, string>(nameof(RoomProperty),defaultBindingMode: BindingMode.TwoWay);
    public string Room
    {
        get => GetValue(RoomProperty);
        set => SetValue(RoomProperty, value);
    }
    public RoomPicker()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<string> WatermarkProperty
    = AvaloniaProperty.Register<RoomPicker, string>(nameof(WatermarkProperty));
    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public async void OnPick(object? sender, RoutedEventArgs args)
    {
        var vm = new PickRoomWindowViewModel();
        var pickRoomWindow = new PickRoomWindow()
        {
            DataContext = vm
        };
        var result = await pickRoomWindow.ShowDialog<string?>((TopLevel.GetTopLevel(this) as Window)!);
        if (result is not null)
        {
            Room = result;
        }
    }
}