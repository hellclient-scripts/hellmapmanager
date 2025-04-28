using System.Collections.Generic;

namespace HellMapManager.Models;



public class ExitLabel
{
    public enum Types
    {
        Command,
        Condition,
        ExCondition,
        To,
        Cost,
    }

    public ExitLabel(Types type, string value)
    {
        Type = type;
        Value = value;
    }
    public bool IsCommand
    {
        get => Type == Types.Command;
    }
    public bool IsCondition
    {
        get => Type == Types.Condition;
    }
    public bool IsExCondition
    {
        get => Type == Types.ExCondition;
    }
    public bool IsTo
    {
        get => Type == Types.To;
    }
    public bool IsCost
    {
        get => Type == Types.Cost;
    }
    public Types Type { get; set; }
    public string Value { get; set; }
}


public class Exit
{
    public Exit() { }
    //路径指令
    public string Command { get; set; } = "";
    //目标房间
    public string To { get; set; } = "";
    public List<Condition> Conditions { get; set; } = [];
    public int Cost { get; set; } = 1;
    public bool Validated()
    {
        return Command != "";
    }
    public Exit Clone()
    {
        return new Exit()
        {
            Command = Command,
            To = To,
            Conditions = Conditions.ConvertAll(m => m.Clone()),
            Cost = Cost,
        };
    }

    public List<ExitLabel> Labels
    {
        get
        {
            var labels = new List<ExitLabel>
            {
                new(ExitLabel.Types.Command, Command),
                new(ExitLabel.Types.To, To)
            };
            foreach (var c in Conditions)
            {
                labels.Add(new ExitLabel(c.Not ? ExitLabel.Types.ExCondition : ExitLabel.Types.Condition, c.Key));
            }
            if (Cost != 1)
            {
                labels.Add(new ExitLabel(ExitLabel.Types.Cost, Cost.ToString()));
            }
            return labels;
        }
    }
    public bool HasCondition
    {
        get => Conditions.Count > 0;
    }
    public string AllConditions
    {
        get => string.Join(",", Conditions.ConvertAll(d => (d.Not ? "! " : "") + d.Key));
    }
    public bool Equal(Exit model)
    {
        if (Command != model.Command || To != model.To || Cost != model.Cost)
        {
            return false;
        }
        if (Conditions.Count != model.Conditions.Count)
        {
            return false;
        }
        for (var i = 0; i < Conditions.Count; i++)
        {
            if (!Conditions[i].Equal(model.Conditions[i]))
            {
                return false;
            }
        }
        return true;
    }
}
