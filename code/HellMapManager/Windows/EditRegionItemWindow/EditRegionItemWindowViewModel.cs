using System;
using HellMapManager.Models;
using HellMapManager.States;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditRegionItemWindow;

public class RIItem(RegionItemType type, string value)
{
    public RegionItemType Type { get; set; } = type;
    public string Value { get; set; } = value;

    public string Label
    {
        get => Type == RegionItemType.Room ? "房间" : "房间分组";
    }
}
public class EditRegionItemWindowViewModel : ObservableObject
{
    public EditRegionItemWindowViewModel(RegionItem? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new RegionItemForm(raw.Clone(), checker) : new RegionItemForm(checker);
    }
    public RegionItem? Raw { get; set; }
    public RegionItemForm Item { get; set; }
    public string Title
    {
        get => Raw is null ? "新元素" : $"编辑元素";
    }
    public static ObservableCollection<RIItem> Items
    {
        get => new([new(RegionItemType.Room, "房间"), new(RegionItemType.Zone, "房间分组")]);
    }

}
