using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Chicken.Common;
using Chicken.Service.Interface;
using Chicken.ViewModel.NewTweet.Base;
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
            parameters.Add(Const.DIRECT_MESSAGE_SKIP_STATUS, Const.DEFAULT_VALUE_TRUE);
            string url = TwitterHelper.GenerateUrlParams(Const.DIRECT_MESSAGES, parameters);
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
                    url = String.Format(Const.RETWEET_CREATE, statusId);
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
        public void PostNewTweet<T>(NewTweetViewModel newTweet, Action<T> callBack)
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
#if RELEASE
            try
            { 
#endif
            var response = dto.Request.EndGetResponse(result);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                using (var reader = new JsonTextReader(streamReader))
                {
                    dto.Result = jsonSerializer.Deserialize<T>(reader);
                }
            }
#if RELEASE
		            }
            catch (Exception e)
            {
            }
            finally
            {  
#endif
            if (dto.CallBack != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    {
                        dto.CallBack(dto.Result);
                    });
            }
#if RELEASE
		            }  
#endif
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

