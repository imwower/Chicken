using System;
using System.Collections.Generic;
using System.Net;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.Service.Interface
{
    public interface ITweetService
    {
        #region Home Page
        void GetTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetMentions<T>(Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetDirectMessages<T>(Action<T> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region profile page
        void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetFollowingIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetFollowerIds<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetUserProfiles<T>(string userIds, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetUserFavorites<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region status page
        void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void AddToFavorites<T>(string statusId, AddToFavoriteActionType action, Action<T> callBack);

        void Retweet<T>(string statusId, RetweetActionType action, Action<T> callBack);

        void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null);

        #endregion

        #region new tweet
        void PostNewTweet<T>(NewTweetModel newTweet, Action<T> callBack);
        #endregion
    }



    public class RequestDataObject<T>
    {
        public HttpWebRequest Request { get; set; }
        public Action<T> CallBack { get; set; }
        public T Result { get; set; }

        public RequestDataObject()
        {
            Result = default(T);
        }
    }
}
