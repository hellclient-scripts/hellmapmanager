using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditRoomWindow;

public class EditRoomWindowViewModel : ObservableObject
{
    public EditRoomWindowViewModel(Room? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new RoomForm(raw.Clone(), Checker) : new RoomForm(Checker);
        View = view;
    }
    public Room? Raw { get; set; }
    public RoomForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建房间"
                : ViewMode ? $"查看房间 {Raw.Name}({Raw.Key})" : $"编辑房间 {Raw.Name}({Raw.Key})";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new RoomForm(Raw, Checker);
            Editing = true;
            OnPropertyChanged(nameof(Editable));
            OnPropertyChanged(nameof(ViewMode));
            OnPropertyChanged(nameof(Editing));
            OnPropertyChanged(nameof(Title));
        }

    }
    public void CancelEdit()
    {
        Editing = false;
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(RoomForm room)
    {
        if (AppState.Main.Current!.Cache.Rooms.ContainsKey(room.Key) && (Raw is null || room.Key != Raw.Key))
        {
            return "房间主键已存在";
        }
        return "";
    }
}
