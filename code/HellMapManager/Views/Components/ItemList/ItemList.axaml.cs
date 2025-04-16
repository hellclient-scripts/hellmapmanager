using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
namespace HellMapManager.Views.Components.ItemList;

public partial class ItemList : UserControl
{
    public ItemList()
    {
        InitializeComponent();
    }
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
AvaloniaProperty.Register<ItemList, IEnumerable>(nameof(ItemsSourceProperty));

    public IEnumerable ItemsSource
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
}