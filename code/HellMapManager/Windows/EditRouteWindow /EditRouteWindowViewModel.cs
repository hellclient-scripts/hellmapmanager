﻿using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Windows.EditDataWindow;
using System.Linq;
using HellMapManager.Windows.NewTagWindow;
using HellMapManager.Windows.EditExitWindow;

namespace HellMapManager.Windows.EditRouteWindow;

public class EditRouteWindowViewModel : ObservableObject
{
    public EditRouteWindowViewModel(Route? raw, bool view)
    {
        Raw = raw;
        Item = (raw is not null) ? new RouteForm(raw, Checker) : new RouteForm(Checker);
        View = view;
    }
    public Route? Raw { get; set; }
    public RouteForm Item { get; set; }
    public bool View { get; set; }
    public bool ViewMode
    {
        get => View && !Editing;
    }
    public string Title
    {
        get =>
            Raw is null
                ? "新建路线"
                : ViewMode ? $"查看路线 {Raw.Key})" : $"编辑路线 {Raw.Key}";
    }
    public bool Editable { get => (Raw is not null) && ViewMode; }
    public bool Editing { get; set; } = false;
    public void EnterEdit()
    {
        if (Raw is not null)
        {
            Item = new RouteForm(Raw, Checker);
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
    public string Checker(RouteForm form)
    {
        if (AppState.Main.Current!.Cache.Aliases.ContainsKey(form.Key) && (Raw is null || form.Key != Raw.Key))
        {
            return "路线主键已存在";
        }
        return "";
    }

}
