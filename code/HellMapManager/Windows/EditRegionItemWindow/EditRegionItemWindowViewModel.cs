using HellMapManager.Models;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditRegionItemWindow;

public class RIItem(RegionItemType type, string value)
{
    public RegionItemType Type { get; } = type;
    public string Value { get; } = value;


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
