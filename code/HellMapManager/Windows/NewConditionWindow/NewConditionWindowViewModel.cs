using HellMapManager.Models;

using CommunityToolkit.Mvvm.ComponentModel;

namespace HellMapManager.Windows.NewConditionWindow;

public class NewConditionWindowViewModel : ObservableObject
{
    public NewConditionWindowViewModel(ValueCondition? raw, ExternalValidator checker)
    {
        Raw = raw;
        Item = (raw is not null) ? new ConditionForm(raw, checker) : new ConditionForm(checker);
    }
    public ValueCondition? Raw { get; set; }
    public ConditionForm Item { get; set; }
    public string Title
    {
        get => "新建条件";
    }
}
