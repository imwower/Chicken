using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
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

        #region binding
        public ICommand AddToFavoriteCommand
        {
            get
            {
                return new DelegateCommand(AddToFavorite);
            }
        }
        #endregion

        public StatusViewModel()
        {
            Title = "Status";
            var baseViewModelList = new List<PivotItemViewModelBase>()
            {
                new StatusDetailViewModel(),
                new StatusRetweetsViewModel(),
                new StatusViewModelBase(),
            };
            PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        public void OnNavigatedTo(string statusId)
        {
            StatusId = statusId;
        }

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            base.MainPivot_LoadedPivotItem(selectedIndex);
            (PivotItems[SelectedIndex] as StatusViewModelBase).StatusId = StatusId;
        }

        public virtual void AddToFavorite(object parameter)
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).AddFavorite(parameter);
        }
    }
}
