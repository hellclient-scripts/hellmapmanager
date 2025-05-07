using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.EditRegionItemWindow;

public delegate string ExternalValidator(RegionItemForm form);
public partial class RegionItemForm : ObservableObject
{
    public RegionItemForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public RegionItemForm(RegionItem model, ExternalValidator checker)
    {
        Raw = model;
        Type = model.Type;
        Value = model.Value;
        Not = model.Not;
        ExternalValidator = checker;
    }
    public RegionItem ToRegionItem()
    {
        return new RegionItem(Type, Value, Not);
    }
    public RegionItem? Raw;
    public ExternalValidator ExternalValidator;
    public RegionItemType Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged(nameof(IsRoom));
        }
    }
    public RegionItemType _type = RegionItemType.Room;
    public string Value { get; set; } = "";
    public bool Not { get; set; } = false;
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Value == "")
        {
            return "值不能为空";
        }
        return "";
    }
    public bool IsRoom
    {
        get => Type == RegionItemType.Room;
    }
}