using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chicken.Model;

namespace Chicken.Service.Interface
{
    public interface ITweetService
    {
        void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null);
        List<Tweet> GetOldTweets();

        List<Tweet> GetNewMentions();
        List<Tweet> GetOldMentions();

        List<DirectMessage> GetDirectMessages();
        List<DirectMessage> GetOldDirectMessages();



        List<Tweet> GetUserTweets(string userId);
        List<Tweet> GetUserOldTweets(string userId);

        #region profile page
        void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);
        /// <summary>
        /// get a profile summary list of sepecific user's following
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callBack"></param>
        /// <param name="parameters"></param>
        void GetFollowingLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null);

        /// <summary>
        /// get a profile summary list of sepecific user's followers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callBack"></param>
        /// <param name="parameters"></param>
        void GetFollowersLists<T>(Action<T> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region status page
        void GetStatusDetail<T>(string statusId, Action<T> callBack, IDictionary<string, object> parameters = null);

        #endregion

    }
}
