namespace HellMapManager.Windows.EditTraceWindow;

using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public delegate string ExternalValidator(TraceForm room);
public partial class TraceForm : ObservableObject
{
    public TraceForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public TraceForm(Trace trace, ExternalValidator checker)
    {
        Key = trace.Key;
        Group = trace.Group;
        Desc = trace.Desc;
        _locations = trace.Locations;
        ExternalValidator = checker;
        Message = trace.Message;
    }
    public Trace ToTrace()
    {
        Arrange();
        return new Trace()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Locations = _locations,
            Message = Message,
        };
    }
    public void Arrange()
    {
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    private List<string> _locations = [];
    public string Message { get; set; } = "";
    public string Rooms
    {
        get => string.Join("\n", _locations);
        set
        {
            _locations = [.. value.Split('\n').Where(x => x != "").Distinct()];
            _locations.Sort((x, y) => x.CompareTo(y));
            OnPropertyChanged(nameof(Rooms));
        }
    }

    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "足迹主键不能为空";
        }

        return "";
    }
}