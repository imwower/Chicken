using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

namespace Chicken.Common
{
    public static class TwitterHelper
    {
        #region const
        static PhoneApplicationFrame frame = Application.Current.RootVisual as PhoneApplicationFrame;
        #region api
        public static string API = "https://wxt2005.org/tapi/o/W699Q6/";

        //api
        public const string STATUSES_HOMETIMELINE = "statuses/home_timeline.json";
        public const string USERS_SHOW = "users/show.json";
        //param
        public const string USER_ID = "user_id";
        #endregion
        #region page name
        public const string ProfilePage = "/View/ProfilePage.xaml";
        #endregion
        #region const
        public const string HTTPGET = "GET";
        public const string HTTPOST = "POST";
        //
        public static string DATETIMEFORMAT = "yyyy-MM-dd  HH:mm:ss";
        //
        const string SOURCEPATTERN = @".*>(?<url>[\s\S]+?)</a>";
        static Regex SourceRegex = new Regex(SOURCEPATTERN);
        const string SOURCEURLPATTERN = @"<a href=\""(?<link>[^\s>]+)\""";
        static Regex SourceUrlRegex = new Regex(SOURCEURLPATTERN);
        #endregion
        #endregion

        #region parse tweet string
        public static string ParseToDateTime(this string date)
        {
            var year = date.Substring(25, 5);
            var month = date.Substring(4, 3);
            var day = date.Substring(8, 2);
            var time = date.Substring(11, 9).Trim();

            var dateTime = string.Format("{0}/{1}/{2} {3}", month, day, year, time);
            var result = DateTime.Parse(dateTime).ToLocalTime().ToString(DATETIMEFORMAT);
            return result;
        }

        public static string ParseToSource(this string source)
        {
            string result = SourceRegex.Match(source).Groups["url"].Value;
            return result;
        }

        public static string ParseToSourceUrl(this string source)
        {
            string result = SourceUrlRegex.Match(source).Groups["link"].Value;
            return result;
        }
        #endregion

        #region generate url
        public static string GenerateUrlParams(string action, IDictionary<string, object> parameters = null)
        {
            StringBuilder sb = new StringBuilder(API).Append(action);
            if (parameters == null || parameters.Count == 0)
            {
                return sb.ToString();
            }
            sb.Append("?");
            foreach (var item in parameters)
            {
                sb.Append(item.Key).Append("=").Append(HttpUtility.UrlEncode(item.Value.ToString())).Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// for NavigationService
        /// </summary>
        /// <param name="pageName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GenerateRelativeUri(string pageName, IDictionary<string, object> parameters = null)
        {
            StringBuilder sb = new StringBuilder(pageName);
            if (parameters == null || parameters.Count == 0)
            {
                return sb.ToString();
            }
            sb.Append("?");
            foreach (var item in parameters)
            {
                sb.Append(item.Key).Append("=").Append(item.Value.ToString()).Append("&");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
        #endregion

        #region navigation service
        public static void NavigateTo(Uri uri)
        {
            frame.Navigate(uri);
        }

        public static void NavigateTo(string uri)
        {
            frame.Navigate(new Uri(uri, UriKind.Relative));
        }
        #endregion
    }
}
