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

namespace Chicken.Service
{
    public class TweetService : ITweetService
    {
        string api = "https://wxt2005.org/tapi/o/W699Q6/";

        public List<Tweet> GetNewTweets()
        {
            var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/hometimeline.json", UriKind.Relative));
            StreamReader streamReader = new StreamReader(reader.Stream);
            string output = streamReader.ReadToEnd();
            var tweets = JsonConvert.DeserializeObject<List<Tweet>>(output);
            return tweets;
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

        public UserProfile GetUserProfile(string userId)
        {
            //https://wxt2005.org/tapi/o/W699Q6/users/show.json?user_id=158678846
            //var reader = System.Windows.Application.GetResourceStream(new Uri("SampleData/userProfile.json", UriKind.Relative));
            //StreamReader streamReader = new StreamReader(reader.Stream);
            //string output = streamReader.ReadToEnd();
            //var userProfile = JsonConvert.DeserializeObject<UserProfile>(output);
            //userProfile.ScreenName = "@" + userProfile.ScreenName;
            //return userProfile;

            //var request = (HttpWebRequest)WebRequest.Create("https://wxt2005.org/tapi/o/W699Q6/statuses/show.json?id=210462857140252672");
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            //string result = string.Empty;
            //var response = request.GetResponse();

            //using (var streamReader = new StreamReader(response.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}
            //var tweet = JsonConvert.DeserializeObject<Tweet>(result);
            //var request = (HttpWebRequest)WebRequest.Create(api + "/users/show.json?user_id=" + userId);
            //request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            //string result = string.Empty;
            //var response = request.EndGetResponse();
            //using (var streamReader = new StreamReader(response.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}
            WebClient webClient = new WebClient();
            UserProfile userProfile = null;
            webClient.OpenReadAsync(new Uri(api + "/users/show.json?user_id=" + userId, UriKind.Absolute));
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(OpenReadCompleted);
            return userProfile;
        }

        void OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //using (StreamReader reader = new StreamReader(e.Result))
            //{
            //    string output = reader.ReadToEnd();
            //    userProfile = JsonConvert.DeserializeObject<UserProfile>(output);
            //    userProfile.ScreenName = "@" + userProfile.ScreenName;
            //}
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
    }
}
