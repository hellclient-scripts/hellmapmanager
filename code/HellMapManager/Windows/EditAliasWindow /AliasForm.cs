namespace HellMapManager.Windows.EditAliasWindow;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
using Microsoft.VisualBasic;

public delegate string ExternalValidator(AliasForm room);
public partial class AliasForm : ObservableObject
{
    public AliasForm(ExternalValidator checker)
    {
        ExternalValidator = checker;
    }
    public AliasForm(Alias alias, ExternalValidator checker)
    {
        Key = alias.Key;
        Value = alias.Value;
        Group = alias.Group;
        Desc = alias.Desc;
        ExternalValidator = checker;
    }
    public Alias ToAlias()
    {
        return new Alias()
        {
            Key = Key,
            Value = Value,
            Group = Group,
            Desc = Desc,
        };
    }
    public void Arrange()
    {
    }
    public ExternalValidator ExternalValidator;
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public string Group { get; set; } = "";
    public string Desc { get; set; } = "";
    public string Validate()
    {
        var err = ExternalValidator(this);
        if (err != "")
        {
            return err;
        }
        if (Key == "")
        {
            return "别名主键不能为空";
        }
        if (Value == "")
        {
            return "房间值不能为空";
        }

        return "";
    }
}