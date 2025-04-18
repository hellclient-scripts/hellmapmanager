using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Windows.EditDataWindow;
using System.Linq;
using HellMapManager.Windows.NewTagWindow;
using HellMapManager.Windows.EditExitWindow;

namespace HellMapManager.Windows.EditAliasWindow;

public class EditAliasWindowViewModel : ObservableObject
{
    public EditAliasWindowViewModel(Alias? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new AliasForm(raw.Clone(), Checker) : new AliasForm(Checker);
        View = view;
    }
    public Alias? Raw { get; set; }
    public AliasForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建别名"
                : ViewMode ? $"查看别名 {Raw.Key})" : $"编辑别名 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new AliasForm(Raw, Checker);
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
    public string Checker(AliasForm room)
    {
        if (AppState.Main.Current!.Cache.Aliases.ContainsKey(room.Key) && (Raw is null || room.Key != Raw.Key))
        {
            return "别名主键已存在";
        }
        return "";
    }

}
