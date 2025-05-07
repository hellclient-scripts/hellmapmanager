namespace HellMapManager.Windows.EditLandmarkWindow;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using HellMapManager.Utils;

public delegate string ExternalValidator(LandmarkForm room);
public partial class LandmarkForm : ObservableObject
{
    public LandmarkForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public LandmarkForm(Landmark landmark, ExternalValidator checker)
    {
        Key = landmark.Key;
        Value = landmark.Value;
        Type = landmark.Type;
        Group = landmark.Group;
        Desc = landmark.Desc;
        ExternalValidator = checker;
    }
    public Landmark ToLandmark()
    {
        Arrange();
        return new Landmark()
        {
            Key = Key,
            Type = Type,
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
    public string Type { get; set; } = "";
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
            return "定位主键不能为空";
        }
        if (Value == "")
        {
            return "值不能为空";
        }

        return "";
    }
    public string UniqueKey

    {
        get => UniqueKeyUtil.Join([Key, Type]);
    }
}