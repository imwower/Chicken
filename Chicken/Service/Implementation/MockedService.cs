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
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            var streamInfo = Application.GetResourceStream(new Uri(url, UriKind.Relative));
            var result = default(T);
            using (var reader = new StreamReader(streamInfo.Stream))
            {
                string s = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<T>(s);
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

        public void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/hometimeline.json";
            HandleWebRequest<T>(url, callBack);
        }

        public List<Tweet> GetOldTweets()
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/hometimeline1.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            for (int i = 0; i < tweets.Count; i++)
            {
                tweets[i].Text = i + "";
            }
            return tweets;
        }

        public List<Tweet> GetNewMentions()
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/mentions.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            for (int i = 0; i < tweets.Count; i++)
            {
                tweets[i].Text = i + " mentions";
            }
            return tweets;
        }

        public List<Tweet> GetOldMentions()
        {
            return GetNewMentions();
        }

        public List<Tweet> GetUserTweets(string userId)
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/user_timeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            foreach (var tweet in tweets)
            {
                tweet.Text = tweets.Count + "User Tweet";
            }
            return tweets;
        }

        public List<Tweet> GetUserOldTweets(string userId)
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/user_timeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            foreach (var tweet in tweets)
            {
                tweet.Text = tweets.Count + "User Old Tweet";
            }
            return tweets;
        }

        public List<DirectMessage> GetDirectMessages()
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/direct_messages.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var messages = JsonConvert.DeserializeObject<List<DirectMessage>>(output);
            foreach (var message in messages)
            {
                message.Text = messages.Count + " message";
            }
            return messages;
        }

        public List<DirectMessage> GetOldDirectMessages()
        {
            return GetDirectMessages();
        }

        #region profile page
        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/friends_list.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowersLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followers.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region status page
        public void GetStatusDetail<T>(string id, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion
    }
}
