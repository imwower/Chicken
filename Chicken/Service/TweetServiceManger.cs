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
#if HTTP
                return new TweetService();
#elif DEBUG
                return new MockedService();
#else
                return new TweetService();
#endif
            }
        }
    }
}
