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
                //return new TweetService();
                return new MockedService();
            }
        }
    }
}
