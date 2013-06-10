namespace Chicken.Common
{
    public class Const
    {
        //public static string API = "https://wxt2005.org/tapi/o/N2X81L/";
        public static string API = "https://blog-lonzhu.rhcloud.com/weixin/o/P26B6O/1.1/";
        public static string API_IMAGE = "https://blog-lonzhu.rhcloud.com/weixin/i/P26B6O/";
        public static string DEFAULT_COUNT_VALUE = "5";
        public static string DEFAULT_COUNT_VALUE_PLUS_ONE = "6";
        public static string QUOTECHARACTER = "RT";

        public static string EMOTIONS_FILE_PATH = "Data/emotions.json";

        #region http method
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";
        public const string BOUNDARY = "-------------CHICKENFORWINDOWSPHONE";
        public const string NEWLINE = "\r\n";
        #endregion

        #region default source and source url
        public const string DefaultSource = "web";
        public const string DefaultSourceUrl = "https://github.com/";
        #endregion

        #region rest api
        public const string DEFAULT_VALUE_TRUE = "true";
        public const string DEFAULT_VALUE_FALSE = "false";
        public const string FOLLOWED_BY = "followed_by";

        #region rest api parameters
        public const string ID = "id";
        public const string USER_ID = "user_id";
        public const string COUNT = "count";
        public const string SINCE_ID = "since_id";
        public const string MAX_ID = "max_id";
        public const string INCLUDE_ENTITIES = "include_entities";
        public const string DIRECT_MESSAGE_SKIP_STATUS = "skip_status";
        public const string CURSOR = "cursor";
        public const string STATUS = "status";
        public const string IN_REPLY_TO_STATUS_ID = "in_reply_to_status_id";
        #endregion

        #region home page
        public const string STATUSES_HOMETIMELINE = "statuses/home_timeline.json";
        public const string STATUSES_MENTIONS_TIMELINE = "statuses/mentions_timeline.json";
        public const string DIRECT_MESSAGES = "direct_messages.json";
        #endregion

        #region profile page
        public const string USERS_SHOW = "users/show.json";
        public const string USER_TIMELINE = "statuses/user_timeline.json";
        public const string USER_FAVORITE = "favorites/list.json";
        public const string USER_FOLLOWING_IDS = "friends/ids.json";
        public const string USER_FOLLOWER_IDS = "followers/ids.json";
        public const string USERS_LOOKUP = "users/lookup.json";
        public const string FRIENDSHIPS_LOOKUP = "friendships/lookup.json";
        #endregion

        #region status page
        public const string STATUSES_SHOW = "statuses/show.json";
        public const string STATUSES_RETWEET_IDS = "statuses/retweeters/ids.json";
        public const string ADD_TO_FAVORITES_CREATE = "favorites/create.json";
        public const string ADD_TO_FAVORITES_DESTROY = "favorites/destroy.json";
        public const string RETWEET_CREATE = "{0}statuses/retweet/{1}.json";

        #endregion

        #region post new tweet
        public const string STATUS_POST_NEW_TWEET = "statuses/update.json";
        public const string STATUS_POST_NEW_TWEET_WITH_MEDIA = "statuses/update_with_media.json";
        #endregion
        #endregion

        #region page name
        public const string MainPage = "/View/MainPage.xaml";
        public const string ProfilePage = "/View/ProfilePage.xaml";
        public const string StatusPage = "/View/StatusPage.xaml";
        public const string NewTweetPage = "/View/NewTweetPage.xaml";

        //public const string RANDOM = "random";
        #endregion

        #region page name enum
        public enum PageNameEnum
        {
            MainPage = 0,
            ProfilePage = 1,
            StatusPage = 2,
            NewTweetPage = 3,
        }
        #endregion

        #region page index enum
        public enum MainPageEnum
        {
            Home = 0,
            Mentions = 1,
            DMs = 2,
        }

        public enum ProfilePageEnum
        {
            ProfileDetail = 0,
            Following = 1,
            Followers = 2,
            Favorites = 3,
        }
        #endregion
    }

    public enum AddToFavoriteActionType
    {
        Create = 1,
        Destroy = 2,
    }

    public enum RetweetActionType
    {
        Create = 1,
        Destroy = 2,
    }

    public enum ScrollTo
    {
        None = 0,
        Top = 1,
        Bottom = 2,
    }

    public enum NewTweetActionType
    {
        None = 0,
        PostNew = 1,
        Reply = 2,
        Quote = 3,
        Mention = 4,
    }
}
