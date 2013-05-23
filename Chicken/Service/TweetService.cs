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
using Chicken.Model;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Chicken.Common;

namespace Chicken.Service
{
    public class TweetService : ITweetService
    {
        public void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = TwitterHelper.GenerateUrlParams(TwitterHelper.STATUSES_HOMETIMELINE);
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

        public void GetUserProfile<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            if (parameters == null || parameters.Count == 0)
            {
                parameters = new Dictionary<string, object>();
            }
            parameters.Add(TwitterHelper.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(TwitterHelper.USERS_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = TwitterHelper.HTTPGET)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToString();
            request.BeginGetResponse(
                (result) =>
                {
                    HttpWebRequest requestResult = (HttpWebRequest)result.AsyncState;
                    var response = requestResult.EndGetResponse(result);
                    T output = default(T);
                    using (var reader = new JsonTextReader(new StreamReader(response.GetResponseStream())))
                    {
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        output = jsonSerializer.Deserialize<T>(reader);
                    }
                    if (callBack != null)
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(
                            () =>
                            {
                                callBack(output);
                            });
                    }
                    request = null;
                    requestResult = null;
                    response.Close();
                    response.Dispose();
                    response = null;
                },
              request);
        }
    }
}
