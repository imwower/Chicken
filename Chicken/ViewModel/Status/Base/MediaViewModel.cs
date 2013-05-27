using Chicken.Model.Entity;

namespace Chicken.ViewModel.Status.Base
{
    public class MediaViewModel
    {
        private Media media;

        public MediaViewModel(Media media)
        {
            this.media = media;
        }

        public string Id
        {
            get
            {
                return media.Id;
            }
        }

        public string Type
        {
            get
            {
                return media.Type;
            }
        }

        public string MediaUrl
        {
            get
            {
                return media.MediaUrl;
            }
        }

        public string MediaUrlThumb
        {
            get
            {
                return media.MediaUrl + ":thumb";
            }
        }

        public string MediaUrlSmall
        {
            get
            {
                return media.MediaUrl + ":small";
            }
        }
    }
}
