namespace HellMapManager.Windows.EditRegionWindow;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public delegate string ExternalValidator(RegionForm room);
public partial class RegionForm : ObservableObject
{
    public RegionForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public RegionForm(Region region, ExternalValidator checker)
    {
        Key = region.Key;
        Group = region.Group;
        Desc = region.Desc;
        Items = [.. region.Items];
        ExternalValidator = checker;
        Message = region.Message;
    }
    public Region ToRegion()
    {
        Arrange();
        return new Region()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Items = new(Items),
            Message = Message,
        };
    }
    public void Arrange()
    {
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Message { get; set; } = "";
    public ObservableCollection<RegionItem> Items { get; set; } = [];

    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "地区主键不能为空";
        }

        return "";
    }
}