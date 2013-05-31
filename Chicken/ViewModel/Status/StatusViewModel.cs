using System.Collections.Generic;
using System.Collections.ObjectModel;
using Chicken.ViewModel.Status.VM;

namespace Chicken.ViewModel.Status
{
    public class StatusViewModel : PivotViewModelBase
    {
        #region properites
        private string statusId;
        public string StatusId
        {
            get
            {
                return statusId;
            }
            set
            {
                statusId = value;
                RaisePropertyChanged("StatusId");
            }
        }

        #endregion

        public StatusViewModel()
        {
            Title = "Status";
            var baseViewModelList = new List<ViewModelBase>()
            {
                new StatusDetailViewModel(),
                new StatusRetweetsViewModel(),
                new StatusViewModelBase(),
            };
            PivotItems = new ObservableCollection<ViewModelBase>(baseViewModelList);
        }

        public void OnNavigatedTo(string statusId)
        {
            StatusId = statusId;
        }

        public void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (!PivotItems[selectedIndex].IsInited)
            {
                (PivotItems[selectedIndex] as StatusViewModelBase).StatusId = StatusId;
                PivotItems[selectedIndex].Refresh();
            }
        }
    }
}
