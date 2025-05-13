using System.Collections.ObjectModel;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.EditExitWindow;

public delegate string ExternalValidator(ExitForm form);
public partial class ExitForm : ObservableObject
{
    public ExitForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public ExitForm(Exit model, ExternalValidator checker)
    {
        Raw = model;
        Command = model.Command;
        To = model.To;
        Conditions = new(model.Conditions);
        Cost = model.Cost;
        ExternalValidator = checker;

    }
    public Exit ToExit()
    {
        return new Exit()
        {
            Command = Command,
            To = To,
            Conditions = [.. Conditions],
            Cost = Cost
        };
    }
    public Exit? Raw;
    public ExternalValidator ExternalValidator;
    public string Command { get; set; } = "";
    public string To { get; set; } = "";
    public ObservableCollection<ValueCondition> Conditions { get; set; } = [];
    public int Cost { get; set; } = 1;

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
    }
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Command == "")
        {
            return "指令不能为空";
        }
        return "";
    }

}