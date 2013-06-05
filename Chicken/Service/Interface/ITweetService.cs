using System;
using System.Collections.Generic;
using System.IO;
using Chicken.Model;
using Chicken.ViewModel.NewTweet.Base;

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

        void GetStatusRetweetIds<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null);

        #endregion

        #region new tweet
        void PostNewTweet<T>(string text, Action<T> callBack, IDictionary<string, object> parameters = null);
        void PostNewTweet<T>(string text, Stream mediaStream, Action<T> callBack, IDictionary<string, object> parameters = null);
        void PostNewTweet<T>(NewTweetViewModel newTweet, Action<object> callBack);
        #endregion

        void UpdateProfileImage(string base64string);
    }
}
