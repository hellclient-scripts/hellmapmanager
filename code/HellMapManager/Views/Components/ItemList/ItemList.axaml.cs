using System;
using System.Collections;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using System.Collections.Specialized;
using CommunityToolkit.Mvvm.ComponentModel;
namespace HellMapManager.Views.Components.ItemList;

public partial class ItemList : UserControl
{
    static ItemList()
    {
        AffectsRender<ItemList>(ItemsSourceProperty, ItemTemplateProperty);
    }
    public ItemList()
    {
        InitializeComponent();
        this.Find<WrapPanel>("Bottons")!.DataContext = new ItemListViewModel();
    }
    public static readonly StyledProperty<IList> ItemsSourceProperty =
AvaloniaProperty.Register<ItemList, IList>(nameof(ItemsSourceProperty));

    public IList ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty
        = AvaloniaProperty.Register<ItemList, IDataTemplate>(nameof(ItemTemplateProperty));
    public IDataTemplate ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public void OnSelectionChanged(object? sender, SelectionChangedEventArgs args)
    {
        var Bottons = this.Find<WrapPanel>("Bottons")!;
        if (Bottons.DataContext is ItemListViewModel vm)
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
    public void OnRemove(object? sender, RoutedEventArgs args)
    {
        var lb = this.Find<ListBox>("Items")!;
        if (lb.SelectedItem is null) return;
        var index = lb.SelectedIndex;
        ItemsSource.RemoveAt(index);
        if (index >= ItemsSource.Count)
        {
            index = ItemsSource.Count - 1;
        }
        lb.SelectedIndex = index;
    }
}