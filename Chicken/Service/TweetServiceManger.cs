using Chicken.Service.Implementation;
using Chicken.Service.Interface;

namespace Chicken.Service
{
    public class TweetServiceManger
    {
        public static ITweetService TweetService
        {
            get
            {
#if DEBUG
                return new MockedService();
#elif HTTP
                return new TweetService();
#else
                return new TweetService();
#endif
            }
        }
    }
}
