using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using Chicken.Common;
using Chicken.Model;
using Chicken.Service.Interface;
using Chicken.ViewModel;
using Newtonsoft.Json;

namespace Chicken.Service.Implementation
{
    public class TweetService : ITweetService
    {
        #region properties
        #region properties
        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        };
        private static JsonSerializer jsonSerializer = JsonSerializer.Create(jsonSettings);
        #endregion
        #endregion

        #region Home Page
        public void GetTweets(Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_HOMETIMELINE, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetMentions(Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_MENTIONS_TIMELINE, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetDirectMessages(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetDirectMessagesSentByMe(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES_SENT_BY_ME, parameters);
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail(User user, Action<UserProfileDetail> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            if (!string.IsNullOrEmpty(user.Id))
            {
                parameters.Add(Const.USER_ID, user.Id);
            }
            else if (!string.IsNullOrEmpty(user.ScreenName))
            {
                parameters.Add(Const.USER_SCREEN_NAME, user.ScreenName);
            }
            parameters.Add(Const.SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest(url, callBack);
        }

        public void FollowOrUnFollow(User user, Action<User> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, user.Id);
            string action = user.IsFollowing ? Const.FRIENDSHIPS_DESTROY : Const.FRIENDSHIPS_CREATE;
            string url = TwitterHelper.GenerateUrlParams(action, parameters);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }

        /// <summary>
        /// comma-separated list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userIdList"></param>
        /// <param name="callBack"></param>
        public void GetFriendshipConnections(string userIdList, Action<Friendships> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, userIdList);
            string url = TwitterHelper.GenerateUrlParams(Const.FRIENDSHIPS_LOOKUP, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetUserTweets(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            if (!string.IsNullOrEmpty(user.Id))
            {
                parameters.Add(Const.USER_ID, user.Id);
            }
            else if (!string.IsNullOrEmpty(user.ScreenName))
            {
                parameters.Add(Const.USER_SCREEN_NAME, user.ScreenName);
            }
            string url = TwitterHelper.GenerateUrlParams(Const.USER_TIMELINE, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetFollowingIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWING_IDS, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetFollowerIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWER_IDS, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetUserProfiles(string userIds, Action<UserProfileList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userIds);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_LOOKUP, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetUserFavorites(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            if (!string.IsNullOrEmpty(user.Id))
            {
                parameters.Add(Const.USER_ID, user.Id);
            }
            else if (!string.IsNullOrEmpty(user.ScreenName))
            {
                parameters.Add(Const.USER_SCREEN_NAME, user.ScreenName);
            }
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FAVORITE, parameters);
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region Status Page
        public void GetStatusDetail(string statusId, Action<Tweet> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_SHOW, parameters);
            HandleWebRequest(url, callBack);
        }

        public void AddToFavorites(string statusId, AddToFavoriteActionType action, Action<Tweet> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, statusId);
            string url = string.Empty;
            switch (action)
            {
                case AddToFavoriteActionType.Destroy:
                    url = Const.ADD_TO_FAVORITES_DESTROY;
                    break;
                case AddToFavoriteActionType.Create:
                default:
                    url = Const.ADD_TO_FAVORITES_CREATE;
                    break;
            }
            url = TwitterHelper.GenerateUrlParams(url, parameters);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }

        public void Retweet(string statusId, RetweetActionType action, Action<Tweet> callBack)
        {
            string url = string.Empty;
            switch (action)
            {
                case RetweetActionType.Create:
                    url = Const.RETWEET_CREATE;
                    break;
            }
            url = string.Format(url, App.Settings.APISettings.Url, statusId);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }

        public void GetStatusRetweetIds(string statusId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_RETWEET_IDS, parameters);
            HandleWebRequest(url, callBack);
        }

        public void DeleteTweet(string statusId, Action<Tweet> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, statusId);
            string url = string.Format(Const.STATUSES_DESTROY, App.Settings.APISettings.Url, statusId);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region new tweet
        public void PostNewTweet(NewTweetModel newTweet, Action<Tweet> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.STATUS, newTweet.Text);
            if (!string.IsNullOrEmpty(newTweet.InReplyToStatusId))
            {
                parameters.Add(Const.IN_REPLY_TO_STATUS_ID, newTweet.InReplyToStatusId);
            }
            string url = TwitterHelper.GenerateUrlParams(Const.STATUS_POST_NEW_TWEET, parameters);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region new message
        public void GetUser(string screenName, Action<User> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_SCREEN_NAME, screenName);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest(url, callBack);
        }

        public void PostNewMessage(NewMessageModel newMessage, Action<DirectMessage> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_SCREEN_NAME, newMessage.User.ScreenName);
            parameters.Add(Const.TEXT, newMessage.Text);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGE_POST_NEW_MESSAGE, parameters);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region my profile
        public void GetMyProfileDetail(Action<UserProfileDetail> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = TwitterHelper.GenerateUrlParams(Const.PROFILE_MYSELF, parameters);
            HandleWebRequest(url, callBack);
        }

        public void UpdateMyProfile(Action<User> callBack, IDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return;
            }
            parameters.Add(Const.SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = TwitterHelper.GenerateUrlParams(Const.PROFILE_UPDATE_MYPROFILE, parameters);
            HandleWebRequest(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region search page
        public void SearchForTweets(string searchQuery, Action<SearchTweetList> callBack, IDictionary<string, object> parameters)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.SEARCH_QUERY, searchQuery);
            if (parameters.ContainsKey(Const.SINCE_ID) || parameters.ContainsKey(Const.MAX_ID))
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE_PLUS_ONE);
            else
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.SEARCH_FOR_TWEETS, parameters);
            HandleWebRequest<SearchTweetList>(url, callBack);
        }

        public void SearchForUsers(string searchQuery, Action<UserProfileList> callBack, IDictionary<string, object> parameters)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.SEARCH_QUERY, searchQuery);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.SEARCH_FOR_USERS, parameters);
            HandleWebRequest<UserProfileList>(url, callBack);
        }
        #endregion

        #region edit api settings
        public void TestAPIUrl(string apiUrl, Action<UserProfileDetail> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = apiUrl + Const.PROFILE_MYSELF;
            url = TwitterHelper.GenerateAPIParams(url, parameters);
            HandleWebRequest(url, callBack);
        }

        public void GetTweetConfiguration(Action<TweetConfiguration> callBack)
        {
            string url = TwitterHelper.GenerateUrlParams(Const.TWEET_CONFIGURATION);
            HandleWebRequest(url, callBack);
        }
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
            where T : ModelBase, new()
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method;
            RequestDataObject<T> data = new RequestDataObject<T>
            {
                Request = request,
                CallBack = callBack,
            };
            request.BeginGetResponse(
                result =>
                {
                    HandleResponse<T>(result);
                }, data);
        }

        private void HandleResponse<T>(IAsyncResult result)
            where T : ModelBase, new()
        {
            var data = result.AsyncState as RequestDataObject<T>;
            #region deserialize
            try
            {
                var response = data.Request.EndGetResponse(result);
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        data.Result = jsonSerializer.Deserialize<T>(reader);
                    }
                }
                response.Close();
            }
            #endregion
            #region exception
            catch (WebException webException)
            {
                try
                {
                    Debug.WriteLine(webException.Message);
                    #region api error
                    using (var stream = webException.Response.GetResponseStream())
                    {
                        if (stream.Length != 0)
                        {
                            using (var streamReader = new StreamReader(stream))
                            {
                                using (var reader = new JsonTextReader(streamReader))
                                {
                                    data.Result = jsonSerializer.Deserialize<T>(reader);
                                }
                            }
                        }
                        else
                            data.Result = GetErrorMessage<T>();
                    }
                    #endregion
                }
                catch (Exception e)
                {
                    data.Result = GetErrorMessage<T>();
                    Debug.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                data.Result = GetErrorMessage<T>();
                Debug.WriteLine(e.Message);
            }
            #endregion
            #region callback
            finally
            {
                if (data.CallBack != null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            if (data.Result.HasError)
                            {
                                App.HandleMessage(new ToastMessage
                                {
                                    Message = data.Result.Errors[0].Message,
                                    Complete = () => data.CallBack(data.Result)
                                });
                            }
                            else
                                data.CallBack(data.Result);
                        });
                }
            }
            #endregion
        }

        #region private
        private static IDictionary<string, object> CheckSinceIdAndMaxId(IDictionary<string, object> parameters)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            if (parameters.ContainsKey(Const.SINCE_ID) || parameters.ContainsKey(Const.MAX_ID))
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE_PLUS_ONE);
            else
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            return parameters;
        }

        private static T GetErrorMessage<T>()
            where T : ModelBase, new()
        {
            return new T
            {
                HasError = true,
                Errors = new List<ErrorMessage>
                {
                    new ErrorMessage
                    {
                        Message = LanguageHelper.GetString("Toast_Msg_UnknowError")
                    }
                }
            };
        }
        #endregion
        #endregion

        private class RequestDataObject<T>
            where T : ModelBase
        {
            public HttpWebRequest Request { get; set; }

            public Action<T> CallBack { get; set; }

            public T Result { get; set; }
        }
    }
}

