namespace HellMapManager.Windows.EditShortcutWindow;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using System.Collections.ObjectModel;

public delegate string ExternalValidator(ShortcutForm room);
public partial class ShortcutForm : ObservableObject
{
    public ShortcutForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public ShortcutForm(Shortcut model, ExternalValidator checker)
    {
        Key = model.Key;
        Command = model.Command;
        To = model.To;
        RoomConditions = [.. model.RoomConditions];
        Conditions = [.. model.Conditions];
        Cost = model.Cost;
        Group = model.Group;
        Desc = model.Desc;
        ExternalValidator = checker;
    }
    public Shortcut ToShortcut()
    {
        Arrange();
        return new Shortcut()
        {
            Key = Key,
            Command = Command,
            To = To,
            RoomConditions = [.. RoomConditions],
            Conditions = [.. Conditions],
            Cost = Cost,
            Group = Group,
            Desc = Desc,
        };
    }
    public void Arrange()
    {
        var list = new List<ValueCondition>(this.Conditions);
        list.Sort(((x, y) =>
        {
            if (x.Not == y.Not)
            {
                return x.Key.CompareTo(y.Key);
            }
            else
            {
                return x.Not.CompareTo(y.Not);
            }
        }));
        Conditions = new ObservableCollection<ValueCondition>(list);
        OnPropertyChanged(nameof(Conditions));
        list = new List<ValueCondition>(this.RoomConditions);
        list.Sort(((x, y) =>
        {
            if (x.Not == y.Not)
            {
                return x.Key.CompareTo(y.Key);
            }
            else
            {
                return x.Not.CompareTo(y.Not);
            }
        }));
        RoomConditions = new ObservableCollection<ValueCondition>(list);
        OnPropertyChanged(nameof(RoomConditions));
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string To { get; set; } = "";
    public string Command { get; set; } = "";
    public ObservableCollection<ValueCondition> RoomConditions { get; set; } = [];

    public ObservableCollection<ValueCondition> Conditions { get; set; } = [];

    public int Cost { get; set; } = 1;
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
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
        if (Command == "")
        {
            return "指令不能为空";
        }

        return "";
    }

}