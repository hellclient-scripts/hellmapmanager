using System;
using HellMapManager.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditRoomWindow;

public class EditRoomWindowViewModel(Room? row, bool view) : ObservableObject
{
    public Room? Raw { get; set; } = row;
    public Room Item { get; set; } = (row is not null) ? row.Clone() : new Room();
    public bool View { get; set; } = view;
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
            Item = Raw.Clone();
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
}
