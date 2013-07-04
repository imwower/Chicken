using Chicken.Model;

namespace Chicken.ViewModel.Base
{
    public class DirectMessageViewModel : TweetViewModel
    {
        public DirectMessageViewModel(DirectMessage directMessage)
            : base(directMessage)
        {
            User.IsVisible = true;
        }
    }
}
