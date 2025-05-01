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
    public ConditionForm(Condition model, ExternalValidator checker)
    {
        Raw = model;
        Key = model.Key;
        Not = model.Not;
        ExternalValidator = checker;
    }
    public Condition ToCondition()
    {
        return new Condition(Key, Not);
    }
    public Condition? Raw;
    public ExternalValidator ExternalValidator;
    public bool Not { get; set; } = false;
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