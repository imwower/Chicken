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

namespace Chicken.Common
{
    public enum HttpMethodType
    {
        GET = 0,
        POST = 1,
    }

    public static class TwitterHelper
    {
        static string api = "https://wxt2005.org/tapi/o/W699Q6/";
        const string USERS = "users/show.json?";

        public static string ParseToDateTime(this string date)
        {
            var year = date.Substring(25, 5);
            var month = date.Substring(4, 3);
            var day = date.Substring(8, 2);
            var time = date.Substring(11, 9).Trim();

            var dateTime = string.Format("{0}/{1}/{2} {3}", month, day, year, time);
            var result = DateTime.Parse(dateTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        public static string ParseToSource(this string source)
        {
            string pattern = @".*>(?<url>[\s\S]+?)</a>";
            Regex reg = new Regex(pattern);
            string result = reg.Match(source).Groups["url"].Value;
            return result;
        }

        public static string ParseToSourceUrl(this string source)
        {
            string pattern = @"<a href=\""(?<link>[^\s>]+)\""";
            Regex reg = new Regex(pattern);
            string result = reg.Match(source).Groups["link"].Value;
            Debug.WriteLine(result);
            return result;
        }

        public static string GenerateUrlParams(IDictionary<string, object> parameters = null)
        {
            StringBuilder sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
