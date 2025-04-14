using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;

using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Collections.Generic;
using Avalonia.Controls.ApplicationLifetimes;
using HellMapManager.Interfaces;
namespace HellMapManager.Services;

public class DialogManager(IClassicDesktopStyleApplicationLifetime desktop) : IAppUI
{
    public IClassicDesktopStyleApplicationLifetime Desktop = desktop;
    public async Task<string> AskLoadFile()
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)Desktop.MainWindow!);

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
    public async Task<string> AskImportRoomsH()
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)Desktop.MainWindow!);

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
    public async Task<string> AskSaveAs()
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)Desktop.MainWindow!);

        var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存地图文件",
            DefaultExtension = "hmm",
            FileTypeChoices = new[] { HMMFileType },
            ShowOverwritePrompt = true,
        });
        return file == null ? "" : file.Path.AbsolutePath;
    }

    public async Task<bool> ConfirmModified()
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
    public async Task<bool> ConfirmImport()
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