namespace HellMapManager.Windows.EditSnapshotWindow;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using HellMapManager.Utils;

public delegate string ExternalValidator(SnapshotForm room);
public partial class SnapshotForm : ObservableObject
{
    public SnapshotForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public SnapshotForm(Snapshot model, ExternalValidator checker)
    {
        Key = model.Key;
        Type = model.Type;
        Value = model.Value;
        Group = model.Group;
        ExternalValidator = checker;
    }
    public Snapshot ToSnapshot()
    {
        Arrange();
        return Snapshot.Create(Key, Type, Value, Group); ;
    }
    public void Arrange()
    {
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Type { get; set; } = "";
    public string Group { get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (!ItemKey.Validate(Key))
        {
            return "主键无效";
        }
        if (Value == "")
        {
            return "值不能为空";
        }

        return "";
    }
    public string UniqueKey

    {
        get => UniqueKeyUtil.Join([Key, Type, Value]);
    }
}