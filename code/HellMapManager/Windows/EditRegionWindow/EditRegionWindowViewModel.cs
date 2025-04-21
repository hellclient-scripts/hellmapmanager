using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;
using System.Linq;
using HellMapManager.Windows.EditRegionItemWindow;

namespace HellMapManager.Windows.EditRegionWindow;

public class EditRegionWindowViewModel : ObservableObject
{
    public EditRegionWindowViewModel(Region? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new RegionForm(raw, Checker) : new RegionForm(Checker);
        View = view;
    }
    public Region? Raw { get; set; }
    public RegionForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建地区"
                : ViewMode ? $"查看地区 {Raw.Key})" : $"编辑地区 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new RegionForm(Raw, Checker);
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
        Item = (Raw is not null) ? new RegionForm(Raw.Clone(), Checker) : new RegionForm(Checker);
        Editing = false;
        OnPropertyChanged(nameof(Item));
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(RegionForm form)
    {
        if (AppState.Main.Current!.Cache.Aliases.ContainsKey(form.Key) && (Raw is null || form.Key != Raw.Key))
        {
            return "地区主键已存在";
        }
        return "";
    }
    public string RegionItemValidator(RegionItemForm form)
    {
        return "";
    }
}
