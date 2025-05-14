using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.NewTagWindow;

public delegate string ExternalValidator(TagForm form);
public partial class TagForm : ObservableObject
{
    public TagForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public TagForm(ValueTag model, ExternalValidator checker)
    {
        Raw = model;
        Key = model.Key;
        Value = model.Value;
        ExternalValidator = checker;
    }
    public ValueTag ToTag()
    {
        return new ValueTag(Key, Value);
    }
    public ValueTag? Raw;
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public int Value { get; set; } = 1;
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "标签不能为空";
        }
        return "";
    }

}