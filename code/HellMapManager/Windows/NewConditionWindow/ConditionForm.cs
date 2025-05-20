using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.NewConditionWindow;

public delegate string ExternalValidator(ConditionForm form);
public partial class ConditionForm : ObservableObject
{
    public ConditionForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public ConditionForm(ValueCondition model, ExternalValidator checker)
    {
        Raw = model;
        Key = model.Key;
        Value = model.Value;
        Not = model.Not;
        ExternalValidator = checker;
    }
    public ValueCondition ToCondition()
    {
        return new ValueCondition(Key, Value, Not);
    }
    public ValueCondition? Raw;
    public ExternalValidator ExternalValidator;
    public bool Not { get; set; } = false;
    public int Value { get; set; } = 1;
    public string Key { get; set; } = "";
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