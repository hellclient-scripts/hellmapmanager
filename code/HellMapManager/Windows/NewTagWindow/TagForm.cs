using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using Microsoft.VisualBasic;
namespace HellMapManager.Windows.NewTagWindow;

public delegate string ExternalValidator(TagForm form);
public partial class TagForm : ObservableObject
{
    public TagForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public TagForm(string model, ExternalValidator checker)
    {
        Raw = model;
        Key = model;
        ExternalValidator = checker;
    }
    public string ToTag()
    {
        return Key;
    }
    public string? Raw;
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "标签不能为空";
        }
        return "";
    }

}