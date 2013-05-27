using Chicken.Model.Entity;
using System.Collections.Generic;

namespace Chicken.ViewModel.Status.Base
{
    public class EntitiesViewModel
    {
        private List<MediaViewModel> mediasViewModel;
        private List<HashTagViewModel> hashTagsViewModel;

        public EntitiesViewModel(Entities entities)
        {
            if (entities != null)
            {
                if (entities.Medias != null)
                {
                    mediasViewModel = new List<MediaViewModel>();
                    foreach (var media in entities.Medias)
                    {
                        var mediaViewModel = new MediaViewModel(media);
                        mediasViewModel.Add(mediaViewModel);
                    }
                }
                if (entities.HashTags != null)
                {
                    hashTagsViewModel = new List<HashTagViewModel>();
                    foreach (var hashTag in entities.HashTags)
                    {
                        hashTagsViewModel.Add(new HashTagViewModel(hashTag));
                    }
                }
            }
        }

        public IList<MediaViewModel> MediasViewModel
        {
            get
            {
                return mediasViewModel;
            }
        }

        public IList<HashTagViewModel> HashTagsViewModel
        {
            get
            {
                return hashTagsViewModel;
            }
        }
    }
}
