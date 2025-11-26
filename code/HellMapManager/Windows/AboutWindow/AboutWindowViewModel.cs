using System;
using HellMapManager.Misc;
using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.AboutWindow;

public class AboutWindowViewModel : ObservableObject
{
    public string Version { get => AppVersion.Current.ToString(); }
    public string URL { get => Links.Homepage; }
}
