using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.ViewModel.Settings.VM;

namespace Chicken.ViewModel.Settings
{
    public class SettingsViewModel : PivotViewModelBase
    {
        public SettingsViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new GeneralSettingsViewModel(),
                new AboutViewModel(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }
    }
}
