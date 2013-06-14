using Chicken.Common;

namespace Chicken.Model
{
    public class NewMessageModel
    {
        public bool IsNew { get; set; }

        public User User { get; set; }

        public string Text { get; set; }
    }
}
