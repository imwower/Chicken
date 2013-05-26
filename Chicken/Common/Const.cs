
namespace Chicken.Common
{
    public class Const
    {
        public static string API = "https://wxt2005.org/tapi/o/W699Q6/";

        #region http method
        public const string HTTPGET = "GET";
        public const string HTTPOST = "POST";
        #endregion

        #region default source and source url
        public const string DefaultSource = "web";
        public const string DefaultSourceUrl = "https://github.com/";
        #endregion

        #region rest api
        public const string STATUSES_HOMETIMELINE = "statuses/home_timeline.json";
        public const string STATUSES_SHOW = "statuses/show.json";
        public const string USERS_SHOW = "users/show.json";

        #region rest api parameters
        public const string ID = "id";
        public const string USER_ID = "user_id";
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
