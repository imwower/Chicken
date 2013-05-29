
namespace Chicken.Common
{
    public class Const
    {
        public static string API = "https://wxt2005.org/tapi/o/W699Q6/";
        public static string DEFAULT_COUNT_VALUE = "5";
        public static string DEFAULT_COUNT_VALUE_PLUS_ONE = "6";

        #region http method
        public const string HTTPGET = "GET";
        public const string HTTPOST = "POST";
        #endregion

        #region default source and source url
        public const string DefaultSource = "web";
        public const string DefaultSourceUrl = "https://github.com/";
        #endregion

        #region rest api

        public const string DEFAULT_VALUE_TRUE = "true";
        public const string DEFAULT_VALUE_FALSE = "false";

        #region rest api parameters
        public const string ID = "id";
        public const string USER_ID = "user_id";
        public const string COUNT = "count";
        public const string SINCE_ID = "since_id";
        public const string MAX_ID = "max_id";
        public const string INCLUDE_ENTITIES = "include_entities";
        public const string DIRECT_MESSAGE_SKIP_STATUS = "skip_status";
        #endregion

        #region home page
        public const string STATUSES_HOMETIMELINE = "statuses/home_timeline.json";
        public const string STATUSES_MENTIONS_TIMELINE = "statuses/mentions_timeline.json";
        public const string DIRECT_MESSAGES = "direct_messages.json";
        #endregion

        #region status page
        public const string STATUSES_SHOW = "statuses/show.json";
        #endregion

        #region profile page
        public const string USERS_SHOW = "users/show.json";
        public const string USER_TIMELINE = "statuses/user_timeline.json";

        #endregion
        #endregion

        #region page name
        public const string MainPage = "/MainPage.xaml";
        public const string ProfilePage = "/View/ProfilePage.xaml";
        public const string StatusPage = "/View/StatusPage.xaml";
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
            Favourites = 3,
        }
        #endregion
    }
}
