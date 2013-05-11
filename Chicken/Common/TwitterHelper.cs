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

namespace Chicken.Common
{
    public static class TwitterHelper
    {
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
    }
}
