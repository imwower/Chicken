
using Chicken.Model;
namespace Chicken.ViewModel.Home.Base
{
    public class RetweetViewModel : TweetViewModel
    {
        private TweetViewModel tweetViewModel;
        private TweetViewModel userTweetViewModel;

        public RetweetViewModel(Retweet retweetedStatus)
            : base(retweetedStatus)
        {
            this.userTweetViewModel = new TweetViewModel(retweetedStatus);
            if (retweetedStatus.RetweetedStatus != null)
            {
                this.tweetViewModel = new TweetViewModel(retweetedStatus.RetweetedStatus);
            }
        }

        /// <summary>
        /// the new tweet who retweets others tweet
        /// </summary>
        public TweetViewModel UserTweetViewModel
        {
            get
            {
                return userTweetViewModel;
            }
        }

        public new string Id
        {
            get
            {
                if (tweetViewModel != null)
                {
                    return tweetViewModel.Id;
                }
                return userTweetViewModel.Id;
            }
        }

        public new string Text
        {
            get
            {
                if (tweetViewModel != null)
                {
                    return tweetViewModel.Text;
                }
                return userTweetViewModel.Text;
            }
        }

        public new UserViewModel User
        {
            get
            {
                if (tweetViewModel != null)
                {
                    return tweetViewModel.User;
                }
                return userTweetViewModel.User;
            }
        }

        public new string CreatedDate
        {
            get
            {
                if (tweetViewModel != null)
                {
                    return tweetViewModel.CreatedDate;
                }
                return userTweetViewModel.CreatedDate;
            }
        }
    }
}
