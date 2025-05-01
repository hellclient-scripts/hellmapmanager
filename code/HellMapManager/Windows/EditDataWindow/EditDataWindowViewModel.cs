using HellMapManager.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.EditDataWindow;

public class EditDataWindowViewModel : ObservableObject
{
    public EditDataWindowViewModel(Data? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new DataForm(raw.Clone(), checker) : new DataForm(checker);
    }
    public Data? Raw { get; set; }
    public DataForm Item { get; set; }
    public string Title
    {
        get => Raw is null ? "新建数据" : $"编辑数据 {Raw.Key}";
    }
}
