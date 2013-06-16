using Chicken.Common;

namespace Chicken.Model
{
    public class NewMessageModel
    {
        /// <summary>
        /// true if send new message,
        /// false if open an existed convsersation.
        /// </summary>
        public bool IsNew { get; set; }

        public User User { get; set; }

        public string Text { get; set; }
    }
}
