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
using Chicken.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chicken.ViewModel.Profile
{
    public class FriendListViewModel : NotificationObject
    {
        private ObservableCollection<FriendProfileViewModel> friendProfileList = new ObservableCollection<FriendProfileViewModel>();
        public ObservableCollection<FriendProfileViewModel> FriendProfileList
        {
            get
            {
                return friendProfileList;
            }
            set
            {
                friendProfileList = value;
                RaisePropertyChanged("FriendProfileList");
            }
        }

        private string nextCursor;
        public string NextCursor
        {
            get
            {
                return nextCursor;
            }
            set
            {
                nextCursor = value;
            }
        }

        private string previousCursor;
        public string PreviousCursor
        {
            get
            {
                return previousCursor;
            }
            set
            {
                previousCursor = value;
            }
        }

        public FriendListViewModel() { }

        public void Refresh(FriendList list)
        {
            if (list.Users != null && list.Users.Count != 0)
            {
                foreach (var userProfile in list.Users)
                {
                    this.FriendProfileList.Add(new FriendProfileViewModel(userProfile));
                }
            }
            this.nextCursor = list.NextCursor;
            this.previousCursor = list.PreviousCursor;
        }
    }

    public class FriendProfileViewModel
    {
        public UserProfile UserProfile;

        public FriendProfileViewModel(UserProfile userProfile)
        {
            this.UserProfile = userProfile;
        }

        public string Name
        {
            get
            {
                return UserProfile.Name;
            }
        }

        public string ScreenName
        {
            get
            {
                return UserProfile.ScreenName;
            }
        }

        public string Id
        {
            get
            {
                return UserProfile.Id;
            }
        }

        public string ProfileImage
        {
            get
            {
                return UserProfile.ProfileImage;
            }
        }

        public string Description
        {
            get
            {
                return UserProfile.Description;
            }
        }

        public string Location
        {
            get
            {
                return UserProfile.Location;
            }
        }

        public string Url
        {
            get
            {
                return UserProfile.Url;
            }
        }

        public string TweetsCount
        {
            get
            {
                return UserProfile.TweetsCount;
            }
        }

        public string FollowingCount
        {
            get
            {
                return UserProfile.FollowingCount;
            }
        }

        public string FollowersCount
        {
            get
            {
                return UserProfile.FollowersCount;
            }
        }

        public string FavourtiesCount
        {
            get
            {
                return UserProfile.FavouritesCount;
            }
        }
    }
}
