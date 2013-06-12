using Chicken.Model;

namespace Chicken.ViewModel.Home.Base
{
    public class DirectMessageViewModel : TweetViewModel
    {
        public bool IsSentByMe { get; set; }

        public DirectMessageViewModel(DirectMessage directMessage)
            : base(directMessage)
        {
        }
    }
}
