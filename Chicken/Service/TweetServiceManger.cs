using Chicken.Service.Implementation;
using Chicken.Service.Interface;

namespace Chicken.Service
{
    public class TweetServiceManager
    {
        private static ITweetService tweetService;

        public static ITweetService TweetService
        {
            get
            {
                if (tweetService == null)
                {
#if HTTP
                    tweetService = new TweetService();
#elif DEBUG
                tweetService = new MockedService();                
#else
                tweetService = new TweetService();
#endif
                }
                return tweetService;
            }
        }
    }
}
