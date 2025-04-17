namespace HellMapManager.Windows.EditRoomWindow;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;

public partial class RoomForm : ObservableObject
{
    public RoomForm()
    {
        Exits = new ObservableCollection<Exit>();
        Data = new ObservableCollection<Data>();
        Tags = new ObservableCollection<string>();
    }
    public RoomForm(Room room) 
    {
        Key = room.Key;
        Name = room.Name;
        Group = room.Group;
        Desc = room.Desc;
        Tags = new ObservableCollection<string>(room.Tags);
        Exits = new ObservableCollection<Exit>(room.Exits);
        Data = new ObservableCollection<Data>(room.Data);
    }
    public Room ToRoom(){
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
    public string Key { get; set; } = "";
    public string Name { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public ObservableCollection<string> Tags { get; set; } = [];
    public ObservableCollection<Exit> Exits { get; set; } = [];
    public ObservableCollection<Data> Data { get; set; } = [];

}