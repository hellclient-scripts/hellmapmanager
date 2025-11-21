using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using HellMapManager.Models;
namespace HellMapManager.Windows.PatchWindow;

public class PatchTabTemplate : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> Templates { get; } = new Dictionary<string, IDataTemplate>();

    public bool Match(object? data)
    {
        return data is PatchTab;
    }

    public Control Build(object? data)
    {
        var key = ((PatchTab)data!).Key;
        if (!Templates.ContainsKey(key))
        {
            return new Panel();
        }
        return Templates[key].Build(data)!;
    }
}

public class DiffItemTemplate : IDataTemplate
{
    [Content]
    public Dictionary<string, IDataTemplate> Templates { get; } = new Dictionary<string, IDataTemplate>();

    public bool Match(object? data)
    {
        return data is IDiffItem;
    }

    public Control Build(object? data)
    {
        string key;
        if (data is IDiffItem i)
        {
            key = i.Mode == DiffMode.Normal ? i.Target : "";
            if (Templates.ContainsKey(key))
            {
                return Templates[key].Build(data)!;
            }
        }
        return new Panel();

    }
}
