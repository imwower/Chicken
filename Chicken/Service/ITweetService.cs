using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chicken.Model;

namespace Chicken.Service
{
    public interface ITweetService
    {
        void GetLastedTweets<T>(Action<T> callBack, IDictionary<string, object> parameters = null);
        List<Tweet> GetOldTweets();

        List<Tweet> GetNewMentions();
        List<Tweet> GetOldMentions();

        List<DirectMessage> GetDirectMessages();
        List<DirectMessage> GetOldDirectMessages();

        void GetUserProfile<T>(string userId, Action<T> callBack, IDictionary<string, object> parameters = null);

        List<Tweet> GetUserTweets(string userId);
        List<Tweet> GetUserOldTweets(string userId);

    }
}
