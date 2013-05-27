using Chicken.Model.Entity;

namespace Chicken.ViewModel.Status.Base
{
    public class HashTagViewModel
    {
        private HashTag hashTag;

        public HashTagViewModel(HashTag hashTag)
        {
            this.hashTag = hashTag;
        }

        public string Text
        {
            get
            {
                return hashTag.Text;
            }
        }
    }
}
