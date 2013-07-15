using System;
using System.Collections.Generic;
using Chicken.Common;
using Chicken.Model;

namespace Chicken.Service.Interface
{
    public interface ITweetService
    {
        #region Home Page
        void GetTweets(Action<TweetList> callBack, IDictionary<string, object> parameters = null);

        void GetMentions(Action<TweetList> callBack, IDictionary<string, object> parameters = null);

        void GetDirectMessages(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null);

        void GetDirectMessagesSentByMe(Action<DirectMessageList> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region profile page
        void GetUserProfileDetail(User user, Action<UserProfileDetail> callBack);

        void FollowOrUnFollow(User user, Action<User> callBack);

        void GetFriendshipConnections(string userIdList, Action<Friendships> callBack);

        void GetUserTweets(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null);

        void GetFollowingIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null);

        void GetFollowerIds(string userId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null);

        void GetUserProfiles(string userIds, Action<UserProfileList> callBack, IDictionary<string, object> parameters = null);

        void GetUserFavorites(User user, Action<TweetList> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region status page
        void GetStatusDetail(string statusId, Action<Tweet> callBack, IDictionary<string, object> parameters = null);

        void AddToFavorites(string statusId, AddToFavoriteActionType action, Action<Tweet> callBack);

        void Retweet(string statusId, RetweetActionType action, Action<Tweet> callBack);

        void GetStatusRetweetIds(string statusId, Action<UserIdList> callBack, IDictionary<string, object> parameters = null);

        void DeleteTweet(string statusId, Action<Tweet> callBack);

        #endregion

        #region new tweet
        void PostNewTweet(NewTweetModel newTweet, Action<Tweet> callBack);
        #endregion

        #region new message
        void GetUser(string screenName, Action<User> callBack);

        void PostNewMessage(NewMessageModel newMessage, Action<DirectMessage> callBack);
        #endregion

        #region my profile page
        void GetMyProfileDetail(Action<UserProfileDetail> callBack);

        void UpdateMyProfile(Action<User> callBack, IDictionary<string, object> parameters);
        #endregion

        #region search page
        void SearchForTweets(string searchQuery, Action<SearchTweetList> callBack, IDictionary<string, object> parameters = null);

        void SearchForUsers(string searchQuery, Action<UserProfileList> callBack, IDictionary<string, object> parameters = null);
        #endregion

        #region edit api settings page
        void TestAPIUrl(string apiUrl, Action<UserProfileDetail> callBack);

        void GetTweetConfiguration(Action<TweetConfiguration> callBack);

        void GetFamous(Action<UserProfileList> callBack);
        #endregion
    }
}
