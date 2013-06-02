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

        public ICommand RetweetCommand
        {
            get
            {
                return new DelegateCommand(Retweet);
            }
        }

        public ICommand ReplyCommand
        {
            get
            {
                return new DelegateCommand(Reply);
            }
        }

        public ICommand QuoteCommand
        {
            get
            {
                return new DelegateCommand(Quote);
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

        private void AddToFavorite()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).AddFavorite();
        }

        private void Retweet()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Retweet();
        }

        private void Reply()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Reply();
        }

        private void Quote()
        {
            (PivotItems[SelectedIndex] as StatusViewModelBase).Quote();
        }
    }
}
