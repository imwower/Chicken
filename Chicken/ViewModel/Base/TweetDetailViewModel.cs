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
            //for retweet,
            //should get the real Id
            if (this.originalTweet != null)
            {
                string id = this.originalTweet.Id;
                this.originalTweet.Tweet.Id = this.tweet.Id;
                this.tweet.Id = id;
            }
            this.profile = Copy(this.Tweet.User);
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
            return new UserProfileViewModel(profile);
        }
    }
}
