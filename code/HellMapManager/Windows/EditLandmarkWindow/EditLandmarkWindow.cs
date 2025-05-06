using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditLandmarkWindow;

public class EditLandmarkWindowViewModel : ObservableObject
{
    public EditLandmarkWindowViewModel(Landmark? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new LandmarkForm(raw.Clone(), Checker) : new LandmarkForm(Checker);
        View = view;
    }
    public Landmark? Raw { get; set; }
    public LandmarkForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建定位"
                : ViewMode ? $"查看定位 {Raw.Key})" : $"编辑定位 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new LandmarkForm(Raw, Checker);
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
        Item = (Raw is not null) ? new LandmarkForm(Raw.Clone(), Checker) : new LandmarkForm(Checker);
        Editing = false;
        OnPropertyChanged(nameof(Item));
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(LandmarkForm model)
    {
        if (AppState.Main.Current!.Cache.Landmarks.ContainsKey(model.UniqueKey) && (Raw is null || model.UniqueKey.ToString() != Raw.UniqueKey().ToString()))
        {
            return "定位主键已存在";
        }
        return "";
    }

}
