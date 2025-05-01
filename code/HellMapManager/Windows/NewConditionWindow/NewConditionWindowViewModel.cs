using HellMapManager.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.NewConditionWindow;

public class NewConditionWindowViewModel : ObservableObject
{
    public NewConditionWindowViewModel(Condition? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new ConditionForm(raw, checker) : new ConditionForm(checker);
    }
    public Condition? Raw { get; set; }
    public ConditionForm Item { get; set; }
    public static string Title
    {
        get => "新建条件";
    }
}
