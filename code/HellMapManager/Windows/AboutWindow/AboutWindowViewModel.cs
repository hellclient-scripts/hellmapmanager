using System;
using HellMapManager.Misc;
using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Cores;

namespace HellMapManager.Windows.AboutWindow;

public class AboutWindowViewModel : ObservableObject
{
    public string Version { get => $"Version:{AppVersion.Current.ToString()}  (API:{MapDatabase.Version.ToString()})"; }
    public string URL { get => Links.Homepage; }
}
