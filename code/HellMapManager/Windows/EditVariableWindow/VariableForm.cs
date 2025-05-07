namespace HellMapManager.Windows.EditVariableWindow;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using HellMapManager.Utils;

public delegate string ExternalValidator(VariableForm room);
public partial class VariableForm : ObservableObject
{
    public VariableForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public VariableForm(Variable model, ExternalValidator checker)
    {
        Key = model.Key;
        Value = model.Value;
        Group = model.Group;
        Desc = model.Desc;
        ExternalValidator = checker;
    }
    public Variable ToVariable()
    {
        Arrange();
        return new Variable()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public void Arrange()
    {
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "变量主键不能为空";
        }
        return "";
    }
}