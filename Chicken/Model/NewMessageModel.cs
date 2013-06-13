using Chicken.Common;

namespace Chicken.Model
{
    public class NewMessageModel
    {
        public NewMessageActionType Type { get; set; }

        public User User { get; set; }

        public string Text { get; set; }
    }
}
