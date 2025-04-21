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
    public ShortcutForm(Shortcut landmark, ExternalValidator checker)
    {
        Key = landmark.Key;
        Command = landmark.Command;
        To = landmark.To;
        RoomConditions = [.. landmark.RoomConditions];
        Conditions = [.. landmark.Conditions];
        Cost = landmark.Cost;
        Group = landmark.Group;
        Desc = landmark.Desc;
        ExternalValidator = checker;
    }
    public Shortcut ToShortcut()
    {
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
        var list = new List<Condition>(this.Conditions);
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
        Conditions = new ObservableCollection<Condition>(list);
        OnPropertyChanged(nameof(Conditions));
        list = new List<Condition>(this.RoomConditions);
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
        RoomConditions = new ObservableCollection<Condition>(list);
        OnPropertyChanged(nameof(RoomConditions));
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string To { get; set; } = "";
    public string Command { get; set; } = "";
    public ObservableCollection<Condition> RoomConditions { get; set; } = [];

    public ObservableCollection<Condition> Conditions { get; set; } = [];

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
        if (Key == "")
        {
            return "捷径主键不能为空";
        }
        if (Command == "")
        {
            return "指令不能为空";
        }

        return "";
    }

}