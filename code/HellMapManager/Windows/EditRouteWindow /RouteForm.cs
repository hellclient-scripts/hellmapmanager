namespace HellMapManager.Windows.EditRouteWindow;

using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public delegate string ExternalValidator(RouteForm room);
public partial class RouteForm : ObservableObject
{
    public RouteForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public RouteForm(Route route, ExternalValidator checker)
    {
        Key = route.Key;
        Group = route.Group;
        Desc = route.Desc;
        _rooms = route.Rooms;
        ExternalValidator = checker;
        Message=route.Message;
    }
    public Route ToRoute()
    {
        return new Route()
        {
            Key = Key,
            Group = Group,
            Desc = Desc,
            Rooms = _rooms,
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
    private List<string> _rooms = [];
    public string Message{ get; set; } = "";
    public string Rooms
    {
        get => string.Join("\n", _rooms);
        set
        {
            _rooms = [.. value.Split('\n').Where(x => x != "")];
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
            return "区域主键不能为空";
        }

        return "";
    }
}