using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditVariableWindow;

public class EditVariableWindowViewModel : ObservableObject
{
    public EditVariableWindowViewModel(Variable? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new VariableForm(raw.Clone(), Checker) : new VariableForm(Checker);
        View = view;
    }
    public Variable? Raw { get; set; }
    public VariableForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建变量"
                : ViewMode ? $"查看变量 {Raw.Key})" : $"编辑变量 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new VariableForm(Raw, Checker);
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
        Item = (Raw is not null) ? new VariableForm(Raw.Clone(), Checker) : new VariableForm(Checker);
        Editing = false;
        OnPropertyChanged(nameof(Item));
        OnPropertyChanged(nameof(Editable));
        OnPropertyChanged(nameof(ViewMode));
        OnPropertyChanged(nameof(Editing));
        OnPropertyChanged(nameof(Title));
    }
    public string Checker(VariableForm model)
    {
        if (AppState.Main.Current!.Cache.Variables.ContainsKey(model.Key) && (Raw is null || model.Key != Raw.Key))
        {
            return "变量主键已存在";
        }
        return "";
    }

}
