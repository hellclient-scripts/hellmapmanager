using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Windows.EditDataWindow;
using System.Linq;
using HellMapManager.Windows.NewTagWindow;
using HellMapManager.Windows.EditExitWindow;

namespace HellMapManager.Windows.EditTraceWindow;

public class EditTraceWindowViewModel : ObservableObject
{
    public EditTraceWindowViewModel(Trace? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new TraceForm(raw, Checker) : new TraceForm(Checker);
        View = view;
    }
    public Trace? Raw { get; set; }
    public TraceForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建足迹"
                : ViewMode ? $"查看足迹 {Raw.Key})" : $"编辑足迹 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new TraceForm(Raw, Checker);
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
        Editing = false;
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(TraceForm form)
    {
        if (AppState.Main.Current!.Cache.Aliases.ContainsKey(form.Key) && (Raw is null || form.Key != Raw.Key))
        {
            return "路线主键已存在";
        }
        return "";
    }

}
