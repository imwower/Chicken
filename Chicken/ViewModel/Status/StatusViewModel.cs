using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Common;
using Chicken.Service;
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

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            if (string.IsNullOrEmpty(StatusId))
                StatusId = IsolatedStorageService.GetObject<string>(PageNameEnum.StatusPage);
            (PivotItems[selectedIndex] as StatusViewModelBase).StatusId = StatusId;
            base.MainPivot_LoadedPivotItem(selectedIndex);
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
