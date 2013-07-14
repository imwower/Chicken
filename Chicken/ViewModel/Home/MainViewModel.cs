using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.ViewModel.Home.VM;

namespace Chicken.ViewModel.Home
{
    public class MainViewModel : PivotViewModelBase
    {
        public MainViewModel()
        {
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }
    }
}
