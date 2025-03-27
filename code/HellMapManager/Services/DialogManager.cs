using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

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
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            // 打开第一个文件的读取流。
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);
            // 将文件的所有内容作为文本读取。
            var fileContent = await streamReader.ReadToEndAsync();
            return fileContent;
        }
        return "";
    }
}