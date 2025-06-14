﻿using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using System.Collections.ObjectModel;


namespace HellMapManager.Windows.UpdateMapWindow;
public class EncodingItem(MapEncoding key, string label) : ObservableObject
{
    public MapEncoding Key { get; } = key;
    public string Label { get; } = label;
}
public class UpdateMapWindowViewModel(MapSettings settings) : ObservableObject
{

    public MapSettings Settings { get; } = settings;
    public static ObservableCollection<EncodingItem> Items
    {
        get => new([new(MapEncoding.Default, "UTF-8"), new(MapEncoding.GB18030, "GB18030")]);
    }
}
