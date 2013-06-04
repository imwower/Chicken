using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service.Interface;
using Newtonsoft.Json;

namespace Chicken.Service.Implementation
{
    public class MockedService : ITweetService
    {
        #region properties
        private static JsonSerializer jsonSerializer = new JsonSerializer();
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            DefaultValueHandling = DefaultValueHandling.Populate
        };
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            var streamInfo = Application.GetResourceStream(new Uri(url, UriKind.Relative));
            var result = default(T);
            using (var reader = new StreamReader(streamInfo.Stream))
            {
                string s = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(s, jsonSettings);
            }
            if (callBack != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        callBack(result);
                    });
            }
        }
        #endregion

        #region Home Page
        public void GetTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/hometimeline.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetMentions<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/mentions.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetDirectMessages<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/direct_messages.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_timeline.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowerIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            //string url = "SampleData/errors.json";
            HandleWebRequest(url, callBack);
        }

        public void GetUserProfiles<T>(string userIds, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/lookup.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserFavorites<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_favourites.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region status page
        public void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }
        public void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        #endregion

        #region new tweet
        public void PostNewTweet<T>(string text, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void PostNewTweet<T>(string text, Stream mediaStream, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion
    }
}
