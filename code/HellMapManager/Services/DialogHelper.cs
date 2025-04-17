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

}