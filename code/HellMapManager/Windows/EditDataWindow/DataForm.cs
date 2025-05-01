using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.EditDataWindow;

public delegate string ExternalValidator(DataForm form);
public partial class DataForm : ObservableObject
{
    public DataForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public DataForm(Data model, ExternalValidator checker)
    {
        Raw = model;
        Key = model.Key;
        Value = model.Value;
        ExternalValidator = checker;
    }
    public Data ToData()
    {
        return new Data(Key, Value);
    }
    public Data? Raw;
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "主键不能为空";
        }
        if (Value == "")
        {
            return "值不能为空";
        }
        return "";
    }

}