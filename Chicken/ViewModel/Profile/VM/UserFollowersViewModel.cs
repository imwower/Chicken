using Chicken.Model;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowersViewModel : ProfileViewModelBase
    {
        public UserFollowersViewModel()
        {
            Header = "Followers";
        }

        private void Refresh()
        {
        }
    }
}
