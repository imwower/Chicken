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
        public void GetTweets(Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/hometimeline.json";
            HandleWebRequest(url, callBack);
        }

        public void GetMentions(Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/mentions.json";
            HandleWebRequest(url, callBack);
        }

        public void GetDirectMessages(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/direct_messages.json";
            HandleWebRequest(url, callBack);
        }

        public void GetDirectMessagesSentByMe(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/direct_messages_sent_by_me.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail(User user, Action<UserProfileDetail> callBack)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest(url, callBack);
        }

        public void FollowOrUnFollow(User user, Action<User> callBack)
        {
            string url = user.IsFollowing ?
                "SampleData/userProfile_unfollow.json" :
                "SampleData/userProfile_follow.json";
            HandleWebRequest(url, callBack);
        }

        public void GetFriendshipConnections(string userIdList, Action<Friendships> callBack)
        {
            string url = "SampleData/friendships.json";
            HandleWebRequest(url, callBack);
        }

        public void GetUserTweets(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_timeline.json";
            HandleWebRequest(url, callBack);
        }

        public void GetFollowingIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        public void GetFollowerIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        public void GetUserProfiles(string userIds, Action<UserProfileList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/lookup.json";
            HandleWebRequest(url, callBack);
        }

        public void GetUserFavorites(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/user_favourites.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region status page
        public void GetStatusDetail(string statusId, Action<Tweet> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }

        public void AddToFavorites(string statusId, AddToFavoriteActionType action, Action<Tweet> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }

        public void Retweet(string statusId, RetweetActionType action, Action<Tweet> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }

        public void GetStatusRetweetIds(string statusId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            string url = "SampleData/followingIds.json";
            HandleWebRequest(url, callBack);
        }

        public void DeleteTweet(string statusId, Action<Tweet> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region new tweet
        public void PostNewTweet(NewTweetModel newTweet, Action<Tweet> callBack)
        {
            string url = "SampleData/tweet.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region new message
        public void GetUser(string screenName, Action<User> callBack)
        {
            string url = "SampleData/userProfile.json";
            HandleWebRequest(url, callBack);
        }

        public void PostNewMessage(NewMessageModel newMessage, Action<DirectMessage> callBack)
        {
            string url = "SampleData/direct_message_post_new_message.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region my profile
        public void GetMyProfileDetail(Action<UserProfileDetail> callBack)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest(url, callBack);
        }

        public void UpdateMyProfile(Action<User> callBack, IDictionary<string, object> parameters)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region edit api settings
        public void TestAPIUrl(string apiUrl, Action<UserProfileDetail> callBack)
        {
            string url = "SampleData/myprofile.json";
            HandleWebRequest(url, callBack);
        }

        public void GetTweetConfiguration(Action<TweetConfiguration> callBack)
        {
            string url = "SampleData/configuration.json";
            HandleWebRequest(url, callBack);
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
