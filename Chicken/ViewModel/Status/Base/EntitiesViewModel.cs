//using System.Collections.Generic;
//using Chicken.Model.Entity;

//namespace Chicken.ViewModel.Status.Base
//{
//    public class EntityViewModelBase
//    {
//        private EntityBase entity;

//        public EntityViewModelBase(EntityBase entity)
//        {
//            this.entity = entity;
//        }

//        public int Index
//        {
//            get
//            {
//                return entity.Indices[0];
//            }
//        }

//        public int Length
//        {
//            get
//            {
//                return entity.Indices[1] - entity.Indices[0];
//            }
//        }
//    }

//    public class EntitiesViewModel
//    {
//        #region private
//        private List<MediaViewModel> mediasViewModel;
//        private List<HashTagViewModel> hashTagsViewModel;
//        private List<UrlViewModel> urlsViewModel;
//        //private List<User
//        #endregion

//        public EntitiesViewModel(Entities entities)
//        {
//            if (entities != null)
//            {
//                if (entities.Medias != null && entities.Medias.Count != 0)
//                {
//                    mediasViewModel = new List<MediaViewModel>();
//                    foreach (var media in entities.Medias)
//                    {
//                        var mediaViewModel = new MediaViewModel(media);
//                        mediasViewModel.Add(mediaViewModel);
//                    }
//                }
//                if (entities.HashTags != null && entities.HashTags.Count != 0)
//                {
//                    hashTagsViewModel = new List<HashTagViewModel>();
//                    foreach (var hashTag in entities.HashTags)
//                    {
//                        hashTagsViewModel.Add(new HashTagViewModel(hashTag));
//                    }
//                }
//                if (entities.Urls != null && entities.Urls.Count != 0)
//                {
//                    urlsViewModel = new List<UrlViewModel>();
//                    foreach (var url in entities.Urls)
//                    {
//                        urlsViewModel.Add(new UrlViewModel(url));
//                    }
//                }
//            }
//        }

//        public List<MediaViewModel> MediasViewModel
//        {
//            get
//            {
//                return mediasViewModel;
//            }
//        }

//        public List<HashTagViewModel> HashTagsViewModel
//        {
//            get
//            {
//                return hashTagsViewModel;
//            }
//        }

//        public List<UrlViewModel> UrlsViewModel
//        {
//            get
//            {
//                return urlsViewModel;
//            }
//        }
//    }
//}
