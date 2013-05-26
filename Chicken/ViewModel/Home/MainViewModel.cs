using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.Common;
using Chicken.ViewModel.Home.VM;
using Chicken.Service;

namespace Chicken.ViewModel.Home
{
    public class MainViewModel : PivotViewModelBase
    {
        public MainViewModel()
        {
            Title = "Chicken";
            var baseViewModelList = new List<ViewModelBase>
            {
                new HomeViewModel(),
                new MentionsViewModel(),
                new DMsPivotViewModel(),
            };
            PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (!PivotItems[selectedIndex].IsInited)
            {
                PivotItems[selectedIndex].Refresh();
            }
        }
    }
}
