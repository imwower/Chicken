using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region MyRegion
        List<Tweet> GetOldTweets();

        List<Tweet> GetNewMentions();
        List<Tweet> GetOldMentions();

        List<DirectMessage> GetDirectMessages();
        List<DirectMessage> GetOldDirectMessages();



        List<Tweet> GetUserTweets(string userId);
        List<Tweet> GetUserOldTweets(string userId);
        #endregion

        #region profile page
        void GetUserProfileDetail<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        void GetUserTweets<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

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
