using System.Collections.Generic;

namespace HellMapManager.Models;



public class ExitLabel
{
    public enum Types
    {
        Command,
        Tag,
        ExTag,
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
        get => this.Type == Types.Command;
    }
    public bool IsTag
    {
        get => this.Type == Types.Tag;
    }
    public bool IsExTag
    {
        get => this.Type == Types.ExTag;
    }
    public bool IsTo
    {
        get => this.Type == Types.To;
    }
    public bool IsCost
    {
        get => this.Type == Types.Cost;
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
    public List<string> Tags { get; set; } = [];
    public List<string> ExTags { get; set; } = [];
    public int Cost { get; set; } = 1;
    public bool Validated()
    {
        return To != "" && Command != "";
    }
    public Exit Clone()
    {
        return new Exit()
        {
            Command = Command,
            To = To,
            Tags = Tags.GetRange(0, Tags.Count),
            ExTags = Tags.GetRange(0, ExTags.Count),
            Cost = Cost,
        };
    }

    public List<ExitLabel> Labels
    {
        get
        {
            var labels = new List<ExitLabel>
            {
                new ExitLabel(ExitLabel.Types.Command, Command),
                new ExitLabel(ExitLabel.Types.To, To)
            };
            foreach (var tag in Tags)
            {
                labels.Add(new ExitLabel(ExitLabel.Types.Tag, tag));
            }
            foreach (var extag in ExTags)
            {
                labels.Add(new ExitLabel(ExitLabel.Types.ExTag, extag));
            }
            if (Cost != 1)
            {
                labels.Add(new ExitLabel(ExitLabel.Types.Cost, Cost.ToString()));
            }
            return labels;
        }
    }
    public bool HasTag
    {
        get => Tags.Count > 0;
    }
    public string AllTags
    {
        get => string.Join(",", Tags);
    }
    public bool HasExTag
    {
        get => ExTags.Count > 0;
    }
    public string AllExTags
    {
        get => string.Join(",", ExTags);
    }
}
