using System.Collections.ObjectModel;
using HellMapManager.Models;
using HellMapManager.ViewModels;

namespace HellMapManager.Windows.PatchWindow;

public partial class PatchWindowViewModel : ViewModelBase
{
    public partial void InitRoutes()
    {
        UpdatedEvent += (sender, args) =>
        {
            OnPropertyChanged(nameof(FilteredRoutes));
        };
    }
    public string RoutesFilter { get; set; } = "";
    public void FilterRoutes()
    {
        OnPropertyChanged(nameof(FilteredRoutes));
    }
    public ObservableCollection<PatchItem> FilteredRoutes
    {
        get
        {
            var models = Patch.Routes.Items;
            if (!string.IsNullOrEmpty(RoutesFilter))
            {
                HellMapManager.Utils.FilterUtil.SplitFilter(RoutesFilter).ForEach(filter =>
                {
                    models = models.FindAll(r =>
                    {
                        if (r.Display is RouteDiff rd)
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
