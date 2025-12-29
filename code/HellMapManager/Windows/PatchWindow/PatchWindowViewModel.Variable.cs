using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitVariables()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredVariables));
        };
    }
    public string VariablesFilter { get; set; } = "";
    public void FilterVariables()
    {
        OnPropertyChanged(nameof(FilteredVariables));
    }
    public ObservableCollection<PatchItem> FilteredVariables
    {
        get
        {
            var models = Patch.Variables.Items;
            if (!string.IsNullOrEmpty(VariablesFilter))
            {
                HellMapManager.Utils.FilterUtil.SplitFilter(VariablesFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is VariableDiff rd)
                        {
                            var model = rd.Model;
                            if (model != null)
                            {
                                return model.Filter(filter);
                            }
                        }
                        return false;

                    });
                });
            }
            return new ObservableCollection<PatchItem>(models);
        }
    }
    
}
