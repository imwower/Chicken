using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service.Interface;
using Newtonsoft.Json;


namespace Chicken.Service.Implementation
{
    public class TweetService : ITweetService
    {
        static JsonSerializer JsonSerializer = new JsonSerializer();

        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToString();
            request.BeginGetResponse(
                (result) =>
                {
                    HttpWebRequest requestResult = (HttpWebRequest)result.AsyncState;
                    var response = requestResult.EndGetResponse(result);
                    T output = default(T);
                    var streamReader = new StreamReader(response.GetResponseStream());
                    try
                    {
#if HTTP
                        using (var reader = new JsonTextReader(streamReader))
                        {
                            output = JsonSerializer.Deserialize<T>(reader);
                        }
#elif DEBUG
                        string responseString = streamReader.ReadToEnd();
                        Console.WriteLine("response string : " + responseString);
                        output = JsonConvert.DeserializeObject<T>(responseString);
#else
                        using (var reader = new JsonTextReader(streamReader))
                        {
                            output = JsonSerializer.Deserialize<T>(reader);
                        }
#endif
                    }
                    catch (Exception e)
                    {
#if DEBUG
                        Console.WriteLine("deserialize error : " + e.ToString());
#endif
                    }
                    finally
                    {
                        if (callBack != null)
                        {
                            Deployment.Current.Dispatcher.BeginInvoke(
                                () =>
                                {
                                    callBack(output);
                                });
                        }
                        if (streamReader != null)
                        {
                            streamReader.Close();
                            streamReader.Dispose();
                            streamReader = null;
                        }
                        request = null;
                        requestResult = null;
                        if (response != null)
                        {
                            response.Close();
                            response.Dispose();
                            response = null;
                        }
                    }
                },
              request);
        }

        public void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
#if DEBUG
            if (parameters == null || parameters.Count == 0)
            {
                parameters = new Dictionary<string, object>(1);
                parameters.Add("count", "2");
            }
#endif
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_HOMETIMELINE, parameters);
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

        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            if (parameters == null || parameters.Count == 0)
            {
                parameters = new Dictionary<string, object>();
            }
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            //throw new NotImplementedException();
        }

        public void GetFollowersLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            //throw new NotImplementedException();
        }
    }
}
