using HellMapManager.Models;
using HellMapManager.States;
using HellMapManager.Windows.NewConditionWindow;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditShortcutWindow;

public class EditShortcutWindowViewModel : ObservableObject
{
    public EditShortcutWindowViewModel(Shortcut? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new ShortcutForm(raw.Clone(), Checker) : new ShortcutForm(Checker);
        View = view;
    }
    public Shortcut? Raw { get; set; }
    public ShortcutForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建捷径"
                : ViewMode ? $"查看捷径 {Raw.Key}" : $"编辑捷径 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new ShortcutForm(Raw, Checker);
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
        Item = (Raw is not null) ? new ShortcutForm(Raw.Clone(), Checker) : new ShortcutForm(Checker);
        Editing = false;
        OnPropertyChanged(nameof(Item));
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(ShortcutForm model)
    {
        if (AppState.Main.Current!.Cache.Shortcuts.ContainsKey(model.Key) && (Raw is null || model.Key != Raw.Key))
        {
            return "捷径主键已存在";
        }
        return "";
    }
    public string ConditionValidator(ConditionForm form)
    {
        foreach (var data in Item.Conditions)
        {
            if (data.Key == form.Key)
            {
                return "条件已存在";
            }
        }
        return "";
    }
    public string RoomConditionValidator(ConditionForm form)
    {
        foreach (var data in Item.Conditions)
        {
            if (data.Key == form.Key)
            {
                return "房间条件已存在";
            }
        }
        return "";
    }

}
