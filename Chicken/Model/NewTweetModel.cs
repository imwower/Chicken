namespace Chicken.Model
{
    public class NewTweetModel
    {
        public string Text { get; set; }

        public int ActionType { get; set; }

        public string InReplyToStatusId { get; set; }

        public string InReplyToUserScreenName { get; set; }

        public string FileName { get; set; }
    }

    public enum NewTweetActionType
    {
        None = 0,
        PostNew = 1,
        Reply = 2,
        Quote = 3,
    }
}
