using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Chicken.Model;
using Chicken.ViewModel.Profile.VM;

namespace Chicken.ViewModel.Profile
{
    public class ProfileViewModel : PivotViewModelBase
    {
        #region properties
        private UserModel user;
        public UserModel User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }
        #endregion

        #region binding
        public ICommand MentionCommand
        {
            get
            {
                return new DelegateCommand(MentionAction);
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
            (PivotItems[selectedIndex] as ProfileViewModelBase).User = user;
            base.MainPivot_LoadedPivotItem(selectedIndex);
        }

        private void MentionAction()
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).Mention();
        }

        private void NewMessageAction(object parameter)
        {
            (PivotItems[SelectedIndex] as ProfileViewModelBase).NewMessage();
        }
    }
}
