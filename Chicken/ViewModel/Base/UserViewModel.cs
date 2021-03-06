﻿using Chicken.Common;
using Chicken.Model;

namespace Chicken.ViewModel.Base
{
    public class UserViewModel : LazyDataItem
    {
        #region private
        private User user;
        #endregion

        public UserViewModel(User user)
        {
            this.user = user;
        }

        public User User
        {
            get
            {
                return user;
            }
        }

        public string Id
        {
            get
            {
                return user.Id;
            }
        }

        public string CreatedDate
        {
            get
            {
                return TwitterHelper.ParseToDateTime(user.CreatedDate);
            }
        }

        public string Name
        {
            get
            {
                return user.Name;
            }
        }

        public string ScreenName
        {
            get
            {
                return user.ScreenName;
            }
        }

        /// <summary>
        /// with @
        /// </summary>
        public string DisplayName
        {
            get
            {
                return user.DisplayName;
            }
        }

        public virtual string ProfileImage
        {
            get
            {
                return user.ProfileImage;
            }
        }

        public bool IsFollowing
        {
            get
            {
                return user.IsFollowing;
            }
        }

        public bool IsVerified
        {
            get
            {
                return user.IsVerified;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return user.IsPrivate;
            }
        }

        public bool IsTranslator
        {
            get
            {
                return user.IsTranslator;
            }
        }

        /// <summary>
        /// for profile page,
        /// only
        /// </summary>
        public bool IsMyself
        {
            get
            {
                return user.IsMyself;
            }
        }
    }

    public class UserProfileViewModel : UserViewModel
    {
        private UserProfile profile;

        public UserProfileViewModel(UserProfile profile)
            : base(profile)
        {
            this.profile = profile;
        }

        public UserProfile UserProfile
        {
            get
            {
                return profile;
            }
        }

        public override string ProfileImage
        {
            get
            {
                return profile.ProfileImage.Replace("_normal", "_bigger");
            }
        }

        public string TruncatedDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(profile.Text) && profile.Text.Length > 60)
                {
                    return profile.Text.Substring(0, 60) + "...";
                }
                return profile.Text;
            }
        }
    }

    public class UserProfileDetailViewModel : UserProfileViewModel
    {
        private UserProfileDetail profile;

        public UserProfileDetailViewModel(UserProfileDetail profile)
            : base(profile)
        {
            this.profile = profile;
            //RaisePropertyChanged("ProfileImage");
        }

        public UserProfileDetail UserProfileDetail
        {
            get
            {
                return profile;
            }
        }

        public override string ProfileImage
        {
            get
            {
                return profile.ProfileImage.Replace("_normal", "");
            }
        }

        public string UserProfileBannerImageWeb
        {
            get
            {
                if (string.IsNullOrEmpty(profile.UserProfileBannerImage))
                {
                    return string.Empty;
                }
                return profile.UserProfileBannerImage + "/web";
            }
        }

        public string Location
        {
            get
            {
                return profile.Location;
            }
        }

        public string Url
        {
            get
            {
                return profile.Url;
            }
        }

        public UserProfileEntities UserProfileEntities
        {
            get
            {
                return profile.UserProfileEntities;
            }
        }

        public string TweetsCount
        {
            get
            {
                return profile.TweetsCount;
            }
        }

        public string FollowingCount
        {
            get
            {
                return profile.FollowingCount;
            }
        }

        public string FollowersCount
        {
            get
            {
                return profile.FollowersCount;
            }
        }

        public string FavoritesCount
        {
            get
            {
                return profile.FavoritesCount;
            }
        }

        /// <summary>
        /// for edit my profile page,
        /// only
        /// </summary>
        public string ExpandedDescription
        {
            get
            {
                string text = profile.Text;
                if (!string.IsNullOrEmpty(profile.Text) &&
                  profile.UserProfileEntities != null &&
                   profile.UserProfileEntities.DescriptionEntities != null &&
                   profile.UserProfileEntities.DescriptionEntities.Urls != null &&
                   profile.UserProfileEntities.DescriptionEntities.Urls.Count != 0)
                {
                    foreach (var u in profile.UserProfileEntities.DescriptionEntities.Urls)
                    {
                        text = text.Replace(u.Text, u.ExpandedUrl);
                    }
                }
                return text;
            }
        }
    }
}
