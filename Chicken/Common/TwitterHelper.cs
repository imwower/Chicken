using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Chicken.Common
{
    public static class TwitterHelper
    {
        #region const
        public static string DATETIMEFORMAT = "yyyy-MM-dd  HH:mm:ss";
        //
        const string SOURCEPATTERN = @".*>(?<url>[\s\S]+?)</a>";
        static Regex SourceRegex = new Regex(SOURCEPATTERN);
        const string SOURCEURLPATTERN = @"<a href=\""(?<link>[^\s>]+)\""";
        static Regex SourceUrlRegex = new Regex(SOURCEURLPATTERN);
        #endregion

        #region parse tweet string
        public static string ParseToDateTime(this string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return string.Empty;
            }
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
            if (string.IsNullOrEmpty(source))
            {
                return Const.DEFAULTSOURCE;
            }
            string result = SourceRegex.Match(source).Groups["url"].Value;
            if (string.IsNullOrEmpty(result))
            {
                return source;
            }
            return result;
        }

        public static string ParseToSourceUrl(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return Const.DEFAULTSOURCEURL;
            }
            string result = SourceUrlRegex.Match(source).Groups["link"].Value;
            if (string.IsNullOrEmpty(result))
            {
                return Const.DEFAULTSOURCEURL;
            }
            return result;
        }
        #endregion

        #region generate url
        public static string GenerateUrlParams(string action, IDictionary<string, object> parameters = null)
        {
            StringBuilder sb = new StringBuilder(Const.API).Append(action);
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
        /// for test api url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GenerateAPIParams(string url, IDictionary<string, object> parameters = null)
        {
            StringBuilder sb = new StringBuilder(url);
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
        #endregion

        public static IDictionary<string, object> GetDictionary(IDictionary<string, object> parameters = null)
        {
            if (parameters == null || parameters.Count == 0)
            {
                parameters = new Dictionary<string, object>();
            }
            return parameters;
        }
    }
}
