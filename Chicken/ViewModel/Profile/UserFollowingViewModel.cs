using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Chicken.Model;

namespace Chicken.ViewModel.Profile
{
    public class UserFollowingViewModel : ProfileViewModelBase
    {
        public UserFollowingViewModel()
        {
            Header = "Following";
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
