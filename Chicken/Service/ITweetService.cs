using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chicken.Model;

namespace Chicken.Service
{
    public interface ITweetService
    {
        List<Tweet> GetNewTweets();
        List<Tweet> GetOldTweets();
    }
}
