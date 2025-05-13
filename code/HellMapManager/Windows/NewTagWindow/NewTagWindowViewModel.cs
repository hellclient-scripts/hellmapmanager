using CommunityToolkit.Mvvm.ComponentModel;
using HellMapManager.Models;
namespace HellMapManager.Windows.NewTagWindow;

public class NewTagWindowViewModel : ObservableObject
{
    public NewTagWindowViewModel(ValueTag? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new TagForm(raw, checker) : new TagForm(checker);
    }
    public ValueTag? Raw { get; set; }
    public TagForm Item { get; set; }
    public static string Title
    {
        get => "新建标签";
    }
}
