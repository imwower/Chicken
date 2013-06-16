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
        public void GetTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_HOMETIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetMentions<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_MENTIONS_TIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetDirectMessages<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetDirectMessagesSentByMe<T>(Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES_SENT_BY_ME, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region profile page
        public void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void FollowOrUnFollow<T>(User user, Action<T> callBack)
        {
            if (user == null)
                return;
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, user.Id);
            string url = user.IsFollowing ?
                TwitterHelper.GenerateUrlParams(Const.FRIENDSHIPS_DESTROY, parameters) :
                TwitterHelper.GenerateUrlParams(Const.FRIENDSHIPS_CREATE, parameters);
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }

        /// <summary>
        /// comma-separated list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userIdList"></param>
        /// <param name="callBack"></param>
        public void GetFriendshipConnections<T>(string userIdList, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_ID, userIdList);
            string url = TwitterHelper.GenerateUrlParams(Const.FRIENDSHIPS_LOOKUP, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_TIMELINE, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowingIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWING_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFollowerIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FOLLOWER_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserProfiles<T>(string userIds, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.USER_ID, userIds);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_LOOKUP, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetUserFavorites<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = CheckSinceIdAndMaxId(parameters);
            parameters.Add(Const.USER_ID, userId);
            string url = TwitterHelper.GenerateUrlParams(Const.USER_FAVORITE, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region Status Page
        public void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void AddToFavorites<T>(string statusId, AddToFavoriteActionType action, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.ID, statusId);
            string url = string.Empty;
            switch (action)
            {
                case AddToFavoriteActionType.Destroy:
                    url = TwitterHelper.GenerateUrlParams(Const.ADD_TO_FAVORITES_DESTROY, parameters);
                    break;
                case AddToFavoriteActionType.Create:
                default:
                    url = TwitterHelper.GenerateUrlParams(Const.ADD_TO_FAVORITES_CREATE, parameters);
                    break;
            }
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }

        public void Retweet<T>(string statusId, RetweetActionType action, Action<T> callBack)
        {
            string url = string.Empty;
            switch (action)
            {
                case RetweetActionType.Create:
                    url = String.Format(Const.RETWEET_CREATE, Const.API, statusId);
                    break;
            }
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }

        public void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            parameters.Add(Const.ID, statusId);
            parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            string url = TwitterHelper.GenerateUrlParams(Const.STATUSES_RETWEET_IDS, parameters);
            HandleWebRequest<T>(url, callBack);
        }
        #endregion

        #region new tweet
        public void PostNewTweet<T>(NewTweetModel newTweet, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.STATUS, newTweet.Text);
            if (!string.IsNullOrEmpty(newTweet.InReplyToStatusId))
            {
                parameters.Add(Const.IN_REPLY_TO_STATUS_ID, newTweet.InReplyToStatusId);
            }
            string url = TwitterHelper.GenerateUrlParams(Const.STATUS_POST_NEW_TWEET, parameters);
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region new message
        public void GetUser<T>(string screenName, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_SCREEN_NAME, screenName);
            parameters.Add(Const.INCLUDE_ENTITIES, Const.DEFAULT_VALUE_FALSE);
            string url = TwitterHelper.GenerateUrlParams(Const.USERS_SHOW, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void GetFriendships<T>(string screenNameList, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_SCREEN_NAME, screenNameList);
            string url = TwitterHelper.GenerateUrlParams(Const.FRIENDSHIPS_LOOKUP, parameters);
            HandleWebRequest<T>(url, callBack);
        }

        public void PostNewMessage<T>(string userName, string text, Action<T> callBack)
        {
            var parameters = TwitterHelper.GetDictionary();
            parameters.Add(Const.USER_SCREEN_NAME, userName);
            parameters.Add(Const.TEXT, text);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGE_POST_NEW_MESSAGE, parameters);
            HandleWebRequest<T>(url, callBack, Const.HTTPPOST);
        }
        #endregion

        #region private method
        private void HandleWebRequest<T>(string url, Action<T> callBack, string method = Const.HTTPGET)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = method.ToString();
            RequestDataObject<T> dto = new RequestDataObject<T>
            {
                Request = request,
                CallBack = callBack,
            };
            request.BeginGetResponse(
                result =>
                {
                    HandleResponse<T>(result);
                }, dto);
        }

        private void HandleResponse<T>(IAsyncResult result)
        {
            var dto = result.AsyncState as RequestDataObject<T>;
            try
            {
                var response = dto.Request.EndGetResponse(result);
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        dto.Result = jsonSerializer.Deserialize<T>(reader);
                    }
                }
            }
            catch (WebException webException)
            {
                using (var streamReader = new StreamReader(webException.Response.GetResponseStream()))
                {
                    using (var reader = new JsonTextReader(streamReader))
                    {
                        dto.Result = jsonSerializer.Deserialize<T>(reader);
                    }
                }
            }
            catch (Exception e)
            {
            }
            finally
            {
                if (dto.CallBack != null)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(
                        () =>
                        {
                            dto.CallBack(dto.Result);
                        });
                }
            }
        }

        public static IDictionary<string, object> CheckSinceIdAndMaxId(IDictionary<string, object> parameters)
        {
            parameters = TwitterHelper.GetDictionary(parameters);
            if (parameters.ContainsKey(Const.SINCE_ID) ||
                parameters.ContainsKey(Const.MAX_ID))
            {
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE_PLUS_ONE);
            }
            else
            {
                parameters.Add(Const.COUNT, Const.DEFAULT_COUNT_VALUE);
            }
            return parameters;
        }
        #endregion
    }
}

