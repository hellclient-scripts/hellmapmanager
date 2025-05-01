using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Avalonia.Controls.ApplicationLifetimes;
using HellMapManager.States;
using System;

namespace HellMapManager.Services;

public class AppUI(AppState appState)
{
    public static readonly AppUI Main = new(new AppState());
    public AppState AppState = appState;
    public IClassicDesktopStyleApplicationLifetime Desktop = new ClassicDesktopStyleApplicationLifetime();
    public static async Task<bool> Confirm(string title, string body)
    {
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions =
                [
                    new() { Name = "是",IsDefault=true },
                    new() { Name = "否",IsCancel=true },
                ],
            ContentTitle = title,
            ContentMessage = body,
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        return choice == "是";
    }
    public static async void Alert(string title, string body)
    {
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions =
                [
                    new() { Name = "确定",IsDefault=true },
                ],
            ContentTitle = title,
            ContentMessage = body,
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        await box.ShowAsync();
        return;
    }
    public async Task<string> AskLoadFile()
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)Desktop.MainWindow!);

        // 启动异步操作以打开对话框。
        var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "打开地图文件",
            AllowMultiple = false,
            FileTypeFilter = [HMMAllFileType],
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

    public static FilePickerFileType HMMFileType
    {
        get => new("文本地图HMM文件")
        {
            Patterns = ["*.hmm"],
        };
    }
    public static FilePickerFileType HMZFileType
    {
        get => new("压缩地图HMZ文件")
        {
            Patterns = ["*.hmz"],
        };
    }
    public static FilePickerFileType HMMAllFileType
    {
        get => new("所有支持的类型")
        {
            Patterns = ["*.hmm", "*.hmz"],
        };
    }

    public async Task<string> AskSaveAs()
    {
        var topLevel = TopLevel.GetTopLevel((Avalonia.Visual)Desktop.MainWindow!);

        var file = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "保存地图文件",
            DefaultExtension = "hmm",
            FileTypeChoices = new[] { HMMFileType, HMZFileType },
            ShowOverwritePrompt = true,
        });
        return file == null ? "" : file.Path.AbsolutePath;
    }

    public async Task<bool> ConfirmModified()
    {
        if (AppState.Current == null || !AppState.Current.Modified)
        {
            return true;
        }
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions =
        [
            new() { Name = "是",IsDefault=true },
                    new() { Name = "否",IsCancel=true },
                ],
            ContentTitle = "文件未保存",
            ContentMessage = "当前文件有未保存的修改，继续操作将丢失所有修改。是否继续",
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        return choice == "是";
    }
    public async Task<bool> ConfirmImport()
    {
        if (AppState.Current == null || !AppState.Current.Modified)
        {
            return true;
        }
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "是",IsDefault=true },
                    new ButtonDefinition { Name = "否",IsCancel=true },
                ],
            ContentTitle = "导入数据",
            ContentMessage = "当前文件有未保存的修改，是否继续导入？",
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        return choice == "是";
    }
    public bool ConfirmedExit = false;
    public async Task<bool> ConfirmExit()
    {
        if (AppState.Current == null || !AppState.Current.Modified)
        {
            ConfirmedExit = true;
            return true;
        }
        var ps = new MessageBoxCustomParams
        {
            ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "是",IsDefault=true },
                    new ButtonDefinition { Name = "否",IsCancel=true },
                ],
            ContentTitle = "退出",
            ContentMessage = "当前文件有未保存的修改，是否退出？",
        };
        var box = MessageBoxManager.GetMessageBoxCustom(ps);
        var choice = await box.ShowAsync();
        ConfirmedExit = (choice == "是");
        return ConfirmedExit;
    }

    public async Task ImportRoomsH()
    {
        var file = await AskImportRoomsH();
        if (file != "")
        {
            try
            {
                AppState.APIImportRoomsHFile(file);
            }
            catch (Exception ex)
            {
                Alert("导入失败", ex.Message);
            }
        }
    }
    public async Task Open()
    {
        var file = await AskLoadFile();
        if (file != "")
        {
            try
            {
                AppState.LoadFile(file);
            }
            catch (Exception ex)
            {
                Alert("打开失败", ex.Message);
            }

        }

    }
    public async Task OpenFile()
    {
        var file = await AskLoadFile();
        if (file != "")
        {
            try
            {
                AppState.LoadFile(file);
            }
            catch (Exception ex)
            {
                Alert("打开失败", ex.Message);
            }

        }
    }
    public async void Revert()
    {
        if (await ConfirmModified())
        {
            try
            {
                AppState.Revert();
            }
            catch (Exception ex)
            {
                Alert("打开失败", ex.Message);
            }
        }
    }


    public async Task SaveAs()
    {
        if (AppState.Current != null)
        {
            var file = await AskSaveAs();
            if (file != "")
            {
                try
                {
                    AppState.SaveFile(file);
                }
                catch (Exception ex)
                {
                    Alert("保存失败", ex.Message);
                }
            }
        }
    }
    public async Task Save()
    {
        if (AppState.Main.Current is not null)
        {
            if (AppState.Main.Current.Path != "")
            {
                try
                {

                    AppState.Main.Save();
                }
                catch (Exception ex)
                {
                    Alert("保存失败", ex.Message);
                }
            }
            else
            {
                await SaveAs();
            }
        }
    }

    public async Task OnOpenRecent(String file)
    {
        if (await ConfirmModified())
        {
            try
            {

                AppState.Main.OpenRecent(file);
            }
            catch (Exception ex)
            {
                Alert("打开失败", ex.Message);
            }
        }
    }

}