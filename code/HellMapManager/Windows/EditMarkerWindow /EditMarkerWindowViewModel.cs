using HellMapManager.Models;
using HellMapManager.Cores;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditMarkerWindow;

public class EditMarkerWindowViewModel : ObservableObject
{
    public EditMarkerWindowViewModel(Marker? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new MarkerForm(raw.Clone(), Checker) : new MarkerForm(Checker);
        View = view;
    }
    public Marker? Raw { get; set; }
    public MarkerForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建标记"
                : ViewMode ? $"查看标记 {Raw.Key}" : $"编辑标记 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new MarkerForm(Raw, Checker);
            Editing = true;
            OnPropertyChanged(nameof(Item));
            OnPropertyChanged(nameof(Editable));
            OnPropertyChanged(nameof(ViewMode));
            OnPropertyChanged(nameof(Editing));
            OnPropertyChanged(nameof(Title));
        }

    }
    public void CancelEdit()
    {
        if (Raw is not null)
        {
            Item = new MarkerForm(Raw.Clone(), Checker);
            Editing = false;
            OnPropertyChanged(nameof(Item));
            OnPropertyChanged(nameof(Editable));
            OnPropertyChanged(nameof(ViewMode));
            OnPropertyChanged(nameof(Editing));
            OnPropertyChanged(nameof(Title));
        }
    }
    public string Checker(MarkerForm model)
    {
        if (AppKernel.MapDatabase.Current!.Cache.Markers.ContainsKey(model.Key) && (Raw is null || model.Key != Raw.Key))
        {
            return "标记主键已存在";
        }
        return "";
    }

}
