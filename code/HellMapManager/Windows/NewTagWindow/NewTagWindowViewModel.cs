using System;
using HellMapManager.Models;
using HellMapManager.States;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.NewTagWindow;

public class NewTagWindowViewModel : ObservableObject
{
    public NewTagWindowViewModel(string? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new TagForm(raw, checker) : new TagForm(checker);
    }
    public string? Raw { get; set; }
    public TagForm Item { get; set; }
    public static string Title
    {
        get => "新建标签";
    }
}
