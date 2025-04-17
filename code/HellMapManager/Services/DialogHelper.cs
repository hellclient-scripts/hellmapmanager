using Avalonia.Controls.Chrome;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia;
using System.Threading.Tasks;

namespace HellMapManager.Services;

public class DialogHelper
{
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

}