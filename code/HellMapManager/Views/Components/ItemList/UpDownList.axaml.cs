using System;
using System.Collections;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using HellMapManager.Services;
namespace HellMapManager.Views.Components.UpDownList;

public partial class UpDownList : UserControl
{
    public UpDownList()
    {
        InitializeComponent();
        this.Find<WrapPanel>("Bottons")!.DataContext = new UpDownListViewModel();
    }
    public static readonly StyledProperty<IList> ItemsSourceProperty =
AvaloniaProperty.Register<UpDownList, IList>(nameof(ItemsSourceProperty));

    public IList ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
    }
    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty
        = AvaloniaProperty.Register<UpDownList, IDataTemplate>(nameof(ItemTemplateProperty));
    public IDataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    public static readonly StyledProperty<IDataTemplate> ActionProperty
    = AvaloniaProperty.Register<UpDownList, IDataTemplate>(nameof(ActionProperty));
    public IDataTemplate Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    public void OnSelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var Bottons = this.Find<WrapPanel>("Bottons")!;
        if (Bottons.DataContext is UpDownListViewModel vm)
        {
            vm.SetSelection(this.Find<ListBox>("Items")!);
        }
    }
    public void OnUp(object? sender, RoutedEventArgs args)
    {
        var lb = this.Find<ListBox>("Items")!;
        if (lb.SelectedItem is null) return;
        var index = lb.SelectedIndex;
        if (index > 0)
        {
            var item = ItemsSource[index];
            ItemsSource.RemoveAt(index);
            ItemsSource.Insert(index - 1, item);
            lb.SelectedIndex = index - 1;
        }
    }
    public void OnDown(object? sender, RoutedEventArgs args)
    {
        var lb = this.Find<ListBox>("Items")!;
        if (lb.SelectedItem is null) return;
        var index = lb.SelectedIndex;
        if (index < ItemsSource.Count - 1)
        {
            var item = ItemsSource[index];
            ItemsSource.RemoveAt(index);
            ItemsSource.Insert(index + 1, item);
            lb.SelectedIndex = index + 1;
        }
    }
}