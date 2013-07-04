using Chicken.Model;

namespace Chicken.ViewModel.Base
{
    public class TweetDetailViewModel : TweetViewModel
    {
        #region private
        private UserProfileViewModel profile;
        #endregion

        public TweetDetailViewModel(Tweet data)
            : base(data)
        {
            this.profile = Copy(this.Tweet.User);
            if (OriginalTweet != null)
                OriginalTweet.User.IsVisible = true;
        }

        public UserProfileViewModel UserProfile
        {
            get
            {
                return profile;
            }
        }

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
            return new UserProfileViewModel(profile);
        }
    }
}
