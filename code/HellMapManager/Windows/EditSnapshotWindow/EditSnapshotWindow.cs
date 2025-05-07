using HellMapManager.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditSnapshotWindow;

public class EditSnapshotWindowViewModel : ObservableObject
{
    public EditSnapshotWindowViewModel(Snapshot? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new SnapshotForm(raw.Clone(), Checker) : new SnapshotForm(Checker);
        View = view;
    }
    public Snapshot? Raw { get; set; }
    public SnapshotForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建快照"
                : ViewMode ? $"查看快照 {Raw.Key}" : $"编辑快照 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new SnapshotForm(Raw, Checker);
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
        Item = (Raw is not null) ? new SnapshotForm(Raw.Clone(), Checker) : new SnapshotForm(Checker);
        Editing = false;
        OnPropertyChanged(nameof(Item));
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(SnapshotForm model)
    {
        return "";
    }

}
