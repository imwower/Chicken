using System.Collections.Generic;

namespace Chicken.Model
{
    public class LatestMessagesModel
    {
        public string SinceId { get; set; }

        public string MaxId { get; set; }

        public string SinceIdByMe { get; set; }

        public string MaxIdByMe { get; set; }

        public List<DirectMessage> Messages { get; set; }

        public LatestMessagesModel()
        {
            Messages = new List<DirectMessage>();
        }
    }

    public class Conversation
    {
        public User User { get; set; }

        public List<DirectMessage> Messages { get; set; }

        public Conversation()
        {
            Messages = new List<DirectMessage>();
        }
    }
}
