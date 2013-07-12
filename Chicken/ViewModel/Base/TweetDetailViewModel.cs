using Chicken.Model;

namespace Chicken.ViewModel.Base
{
    public class TweetDetailViewModel : TweetViewModel
    {
        #region private
        private UserProfileViewModel profile;
        #endregion

        public TweetDetailViewModel(Tweet data, bool isVisible)
            : base(data)
        {
            this.profile = Copy(this.Tweet.User);
            IsVisible = isVisible;
        }

        public override bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }
            set
            {
                if (OriginalTweet != null)
                    OriginalTweet.User.IsVisible = value;
                base.IsVisible = value;
            }
        }

        public UserProfileViewModel UserProfile
        {
            get
            {
                return profile;
            }
        }

        /// <summary>
        /// convert user to profile
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private static UserProfileViewModel Copy(User user)
        {
            var profile = new UserProfile
            {
                Id = user.Id,
                CreatedDate = user.CreatedDate,
                Name = user.Name,
                ScreenName = user.ScreenName,
                ProfileImage = user.ProfileImage,
                IsFollowing = user.IsFollowing,
                IsVerified = user.IsVerified,
                IsPrivate = user.IsPrivate,
                IsTranslator = user.IsTranslator,
            };
            return new UserProfileViewModel(profile, true);
        }
    }
}
