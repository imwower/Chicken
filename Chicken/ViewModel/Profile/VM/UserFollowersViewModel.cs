using Chicken.Model;
using Chicken.ViewModel.Profile.Base;

namespace Chicken.ViewModel.Profile.VM
{
    public class UserFollowersViewModel : ProfileViewModelBase
    {
        public UserFollowersViewModel()
        {
            Header = "Followers";
            FriendList = new FriendListViewModel();
        }

        public override void Refresh()
        {
            base.Refresh();
            TweetService.GetFollowingLists<FriendList>(
                (result) =>
                {
                    FriendList.Refresh(result);
                });
            base.Refreshed();
        }
    }
}
