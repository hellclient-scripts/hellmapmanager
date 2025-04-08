using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Collections.Generic;
namespace HellMapManager.Services;

public class DialogManager
{
    public static async Task<String> LoadFile(object sender)
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)sender);

        // 启动异步操作以打开对话框。
        var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "打开地图文件",
            AllowMultiple = false,
            FileTypeFilter = new[] { HMMFileType },
        });

        if (files.Count >= 1)
        {
            return files[0].Path.AbsolutePath;
        }
        return "";
    }
    public static async Task<String> ImportRoomsH(object sender)
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)sender);

        // 启动异步操作以打开对话框。
        var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "导入UTF8格式的rooms.h文件",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            return files[0].Path.AbsolutePath;
        }
        return "";
    }

    public static FilePickerFileType HMMFileType = new FilePickerFileType("HMM文件")
    {
        Patterns = new[] { "*.hmm" },
    };
    public static async Task<String> SaveAs(object sender)
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)sender);

        var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存地图文件",
            DefaultExtension = "hmm",
            FileTypeChoices = new[] { HMMFileType },
            ShowOverwritePrompt = true,
        });
        return file == null ? "" : file.Path.AbsolutePath;
    }

    public static async Task<bool> ConfirmModifiedDialog()
    {
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "是",IsDefault=true },
                    new ButtonDefinition { Name = "否",IsCancel=true },
                },
            ContentTitle = "文件未保存",
            ContentMessage = "当前文件有未保存的修改，继续操作将丢失所有修改。是否继续",
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        return choice == "是";

    }
    public static async Task<bool> ConfirmImportDialog()
    {
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions = new List<ButtonDefinition>
                {
                    new ButtonDefinition { Name = "是",IsDefault=true },
                    new ButtonDefinition { Name = "否",IsCancel=true },
                },
            ContentTitle = "导入数据",
            ContentMessage = "当前文件有未保存的修改，是否继续导入？",
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        return choice == "是";
    }

}