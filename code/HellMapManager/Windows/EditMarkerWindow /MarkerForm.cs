namespace HellMapManager.Windows.EditMarkerWindow;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public delegate string ExternalValidator(MarkerForm room);
public partial class MarkerForm : ObservableObject
{
    public MarkerForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public MarkerForm(Marker model, ExternalValidator checker)
    {
        Key = model.Key;
        Value = model.Value;
        Group = model.Group;
        Desc = model.Desc;
        Message=model.Message;
        ExternalValidator = checker;
    }
    public Marker ToMarker()
    {
        Arrange();
        return new Marker()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
            Message = Message,
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
    public string Message{ get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "标记主键不能为空";
        }
        if (Value == "")
        {
            return "房间值不能为空";
        }

        return "";
    }
}