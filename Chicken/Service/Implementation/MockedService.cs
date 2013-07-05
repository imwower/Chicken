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

        public void GetDirectMessagesSentByMe<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/direct_messages_sent_by_me.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail<T>(User user, Action<T> callBack)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void FollowOrUnFollow<T>(User user, Action<T> callBack)
        {
            if (user.IsFollowing)
            {
                string url = "SampleData/userProfile_unfollow.json";
                HandleWebRequest<T>(url, callBack);
            }
            else
            {
                string url = "SampleData/userProfile_follow.json";
                HandleWebRequest<T>(url, callBack);
            }
        }

        public void GetFriendshipConnections<T>(string userIdList, Action<T> callBack)
        {
            string url = "SampleData/friendships.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserTweets<T>(User user, Action<T> callBack, IDictionary<string, object> parameters = null)
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

        public void GetUserFavorites<T>(User user, Action<T> callBack, IDictionary<string, object> parameters = null)
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

        public void AddToFavorites<T>(string statusId, AddToFavoriteActionType action, Action<T> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void Retweet<T>(string statusId, RetweetActionType action, Action<T> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        public void DeleteTweet<T>(string statusId, Action<T> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region new tweet
        public void PostNewTweet<T>(NewTweetModel newTweet, Action<T> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region new message
        public void GetUser<T>(string screenName, Action<T> callBack)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFriendships<T>(string screenNameList, Action<T> callBack)
        {
            string url = "SampleData/friendships.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void PostNewMessage<T>(string userName, string text, Action<T> callBack)
        {
            //string url = "SampleData/direct_message_post_new_message.json";
            string url = "SampleData/not_following_you.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region my profile
        public void GetMyProfileDetail<T>(Action<T> callBack)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void UpdateMyProfile<T>(Action<T> callBack, IDictionary<string, object> parameters)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region edit api settings
        public void TestAPIUrl<T>(string apiUrl, Action<T> callBack)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest<T>(url, callBack);
        }

        public void GetTweetConfiguration<T>(Action<T> callBack)
        {
            string url = "SampleData/configuration.json";
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack)
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
    }
}
