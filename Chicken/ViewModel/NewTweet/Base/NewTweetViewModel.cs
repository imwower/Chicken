
namespace Chicken.ViewModel.NewTweet.Base
{
    public enum NewTweetActionType
    {
        None =0,
        PostNew =1,
        Reply =2,
        Quote =3,
    }

    public class NewTweetViewModel : NotificationObject
    {
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
            }
        }

        public NewTweetActionType ActionType { get; set; }
        public string InReplyToTweetId { get; set; }
        public string InReplyToUserScreenName { get; set; }
    }
}
