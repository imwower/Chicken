using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : PivotViewModelBase
    {
        #region properties
        private string userId;
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                RaisePropertyChanged("UserId");
            }
        }
        #endregion

        #region binding
        public ICommand MentionCommand
        {
            get
            {
                return new DelegateCommand(Mention);
            }
        }

        public ICommand MessageCommand
        {
            get
            {
                return new DelegateCommand(NewMessageAction);
            }
        }
        #endregion

        public ProfileViewModel()
        {
            Title = "Profile";
            var baseViewModelList = new List<PivotItemViewModelBase>
            {
                new ProfileDetailViewModel(),
                new UserTweetsViewModel(),
                new UserFollowingViewModel(),
                new UserFollowersViewModel(),
                new UserFavoritesViewModel(),
            };
            this.PivotItems = new ObservableCollection<PivotItemViewModelBase>(baseViewModelList);
        }

        public override void MainPivot_LoadedPivotItem(int selectedIndex)
        {
            (PivotItems[selectedIndex] as ProfileViewModelBase).UserId = userId;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }

        private void Mention(object parameter)
        {
            //TODO
        }

        private void NewMessageAction(object parameter)
        {
            //TODO
        }
    }
}
