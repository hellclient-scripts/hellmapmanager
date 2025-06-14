namespace HellMapManager.Windows.EditRoomWindow;
using System.Collections.ObjectModel;
using System.Linq;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public delegate string ExternalValidator(RoomForm room);
public partial class RoomForm : ObservableObject
{
    public RoomForm(ExternalValidator checker)
    {
        Exits = [];
        Data = [];
        Tags = [];
        ExternalValidator = checker;
    }
    public RoomForm(Room room, ExternalValidator checker)
    {
        Key = room.Key;
        Name = room.Name;
        Group = room.Group;
        Desc = room.Desc;
        Tags = new ObservableCollection<ValueTag>(room.Tags);
        Exits = new ObservableCollection<Exit>(room.Exits);
        Data = new ObservableCollection<Data>(room.Data);
        ExternalValidator = checker;
    }
    public Room ToRoom()
    {
        Arrange();
        return new Room()
        {
            Key = Key,
            Name = Name,
            Group = Group,
            Desc = Desc,
            Tags = [.. Tags],
            Exits = [.. Exits],
            Data = [.. Data]
        };
    }
    public void Arrange()
    {
        Data = new ObservableCollection<Data>(this.Data.OrderBy(x => x.Key));
        Tags = new ObservableCollection<ValueTag>(this.Tags.OrderBy(x => x.Key));
        foreach (var exit in Exits)
        {
            exit.Arrange();
        }
        OnPropertyChanged(nameof(Data));
        OnPropertyChanged(nameof(Tags));
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public ObservableCollection<ValueTag> Tags { get; set; } = [];
    public ObservableCollection<Exit> Exits { get; set; } = [];
    public ObservableCollection<Data> Data { get; set; } = [];

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
        return "";
    }

}