namespace Chicken.Common
{
    public class Const
    {
        public static string testAPI = "http://chicken4wp.hostingsiteforfree.com/o/6FUQI7/1.1/";
        //public static string testAPI = "https://blog-lonzhu.rhcloud.com/weixin/o/9KZ895/1.1/";
        //public static string testAPI = "https://wxt2005.org/tapi/o/HLPI75/1.1/";

        //chicken4wp
        //public static string testAPI = "https://blog-lonzhu.rhcloud.com/weixin/o/391VC9/1.1/";
        //public static string API_IMAGE = "https://blog-lonzhu.rhcloud.com/weixin/i/P26B6O/";
        public static string DEFAULT_COUNT_VALUE = "20";
        public static string DEFAULT_COUNT_VALUE_PLUS_ONE = "21";
        public static string QUOTECHARACTER = "RT";

        #region defalut value
        public const string DEFAULTSOURCE = "web";
        public const string DEFAULTSOURCEURL = "https://github.com/";

        public const int MaxCharLength = 140;
        #endregion

        #region http method
        public const string HTTPGET = "GET";
        public const string HTTPPOST = "POST";
        public const string BOUNDARY = "-------------CHICKENFORWINDOWSPHONE";
        public const string NEWLINE = "\r\n";
        #endregion

        #region rest api
        public const string DEFAULT_VALUE_TRUE = "true";
        public const string DEFAULT_VALUE_FALSE = "false";
        public const string FOLLOWED_BY = "followed_by";

        #region rest api parameters
        public const string ID = "id";
        public const string USER_ID = "user_id";
        public const string USER_SCREEN_NAME = "screen_name";
        public const string COUNT = "count";
        public const string SINCE_ID = "since_id";
        public const string MAX_ID = "max_id";
        public const string INCLUDE_ENTITIES = "include_entities";
        public const string DIRECT_MESSAGE_SKIP_STATUS = "skip_status";
        public const string CURSOR = "cursor";
        public const string STATUS = "status";
        public const string IN_REPLY_TO_STATUS_ID = "in_reply_to_status_id";
        public const string TEXT = "text";
        public const string SKIP_STATUS = "skip_status";
        //update my profile
        public const string USER_NAME = "name";
        public const string URL = "url";
        public const string LOCATION = "location";
        public const string DESCRIPTION = "description";
        #endregion

        #region home page
        public const string STATUSES_HOMETIMELINE = "statuses/home_timeline.json";
        public const string STATUSES_MENTIONS_TIMELINE = "statuses/mentions_timeline.json";
        public const string DIRECT_MESSAGES = "direct_messages.json";
        public const string DIRECT_MESSAGES_SENT_BY_ME = "direct_messages/sent.json";
        #endregion

        #region profile page
        public const string USERS_SHOW = "users/show.json";
        public const string FRIENDSHIPS_CREATE = "friendships/create.json";
        public const string FRIENDSHIPS_DESTROY = "friendships/destroy.json";
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

        #region new message
        public const string DIRECT_MESSAGE_POST_NEW_MESSAGE = "direct_messages/new.json";
        #endregion

        #region my profile
        public const string PROFILE_MYSELF = "account/verify_credentials.json";
        public const string PROFILE_UPDATE_MYPROFILE = "account/update_profile.json";
        #endregion

        #region api settings
        public const string TWEET_CONFIGURATION = "help/configuration.json";
        #endregion

        #endregion

        #region page name
        public const string HomePage = "/View/HomePage.xaml";
        public const string ProfilePage = "/View/ProfilePage.xaml";
        public const string StatusPage = "/View/StatusPage.xaml";
        public const string NewTweetPage = "/View/NewTweetPage.xaml";
        public const string NewMessagePage = "/View/NewMessagePage.xaml";
        public const string EditMyProfilePage = "/View/EditMyProfilePage.xaml";
        public const string SettingsPage = "/View/SettingsPage.xaml";
        public const string APISettingsPage = "/View/APISettingsPage.xaml";

        #endregion
    }
    #region page name enum
    public enum PageNameEnum
    {
        HomePage = 0,
        ProfilePage = 1,
        StatusPage = 2,
        NewTweetPage = 3,
        NewMessagePage = 4,
        EditMyProfilePage = 5,
        SettingsPage = 6,
        APISettingsPage = 7,
    }
    #endregion

    #region page index enum
    //public enum MainPageEnum
    //{
    //    Home = 0,
    //    Mentions = 1,
    //    DMs = 2,
    //}

    public enum ProfilePageEnum
    {
        ProfileDetail = 0,
        Following = 1,
        Followers = 2,
        Favorites = 3,
    }
    #endregion

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

    public enum AppBarState
    {
        Default = 0,
        AddImage = 1,
        AddMention = 2,
        AddEmotion = 3,

        ProfileDefault = 10,
        ProfileWithRefresh = 11,

        MyProfileDefault = 20,
        MyProfileWithEdit = 21,

        StatusPageDefault = 30,
        StatusPageWithDelete = 31,
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
