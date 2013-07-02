using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;
using Chicken.Service;

namespace Chicken.Controls
{
    public class AutoRichTextBox : RichTextBox
    {
        public static DependencyProperty TweetDataProperty =
            DependencyProperty.Register("TweetData", typeof(TweetBase), typeof(AutoRichTextBox), new PropertyMetadata(TweetDataPropertyChanged));

        public TweetBase TweetData
        {
            get
            {
                return (TweetBase)GetValue(TweetDataProperty);
            }
            set
            {
                SetValue(TweetDataProperty, value);
            }
        }

        public static void TweetDataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            #region init
            var textBox = sender as AutoRichTextBox;
            var tweet = e.NewValue as TweetBase;
            if (textBox == null || tweet == null)
            {
                return;
            }
            textBox.Blocks.Clear();
            string text = tweet.Text;
            var paragraph = new Paragraph();
            var entities = new List<EntityBase>();
            #region tweet
            if (tweet.Entities != null)
            {
                #region add entity
                var entityList = new List<EntityBase>();
                if (tweet.Entities.UserMentions != null)
                    entityList.AddRange(tweet.Entities.UserMentions.Cast<EntityBase>());
                if (tweet.Entities.HashTags != null)
                    entityList.AddRange(tweet.Entities.HashTags.Cast<EntityBase>());
                if (tweet.Entities.Urls != null)
                    entityList.AddRange(tweet.Entities.Urls.Cast<EntityBase>());
                if (tweet.Entities.Medias != null)
                    entityList.AddRange(tweet.Entities.Medias.Cast<EntityBase>());
                #endregion
                var parsedList = new List<EntityBase>();
                parsedList.AddRange(TwitterHelper.ParseUserMentions(text));
                parsedList.AddRange(TwitterHelper.ParseHashTags(text));
                parsedList.AddRange(TwitterHelper.ParseUrls(text));
                var results = Select(entityList, parsedList);
                entities.AddRange(results);
            }
            #endregion
            #region profile
            else if (e.NewValue is UserProfileDetail)
            {
                var profile = e.NewValue as UserProfileDetail;
                var mentions = TwitterHelper.ParseUserMentions(profile.Text);
                entities.AddRange(mentions);
                var hashtags = TwitterHelper.ParseHashTags(profile.Text);
                entities.AddRange(hashtags);
                #region url
                if (profile.UserProfileEntities != null &&
                    profile.UserProfileEntities.DescriptionEntities != null &&
                    profile.UserProfileEntities.DescriptionEntities.Urls != null)
                {
                    var parsedUrls = TwitterHelper.ParseUrls(profile.Text);
                    var results = Select(profile.UserProfileEntities.DescriptionEntities.Urls.Cast<EntityBase>(), parsedUrls);
                    entities.AddRange(results);
                }
                #endregion
            }
            #endregion
            #endregion
            #region none
            if (entities.Count == 0)
            {
                paragraph.Inlines.Add(new Run
                {
                    Text = HttpUtility.HtmlDecode(text)
                });
                textBox.Blocks.Add(paragraph);
            }
            #endregion
            #region add
            else
            {
                #region replace
                int index = 0;
                foreach (var entity in entities.OrderBy(v => v.Index))
                {
                    #region starter
                    if (index < entity.Index)
                    {
                        paragraph.Inlines.Add(new Run
                        {
                            Text = HttpUtility.HtmlDecode(text.Substring(index, entity.Index - index))
                        });
                        index = entity.Index;
                    }
                    #endregion
                    var hyperlink = new Hyperlink();
                    hyperlink.TextDecorations = null;
                    hyperlink.Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                    #region entity
                    switch (entity.EntityType)
                    {
                        #region mention, hashtag
                        case EntityType.UserMention:
                        case EntityType.HashTag:
                            hyperlink.CommandParameter = entity;
                            hyperlink.Click += Hyperlink_Click;
                            hyperlink.Inlines.Add(entity.Text);
                            break;
                        #endregion
                        #region media, url
                        case EntityType.Media:
                            hyperlink.NavigateUri = new Uri(entity.MediaUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(entity.TruncatedUrl);
                            break;
                        case EntityType.Url:
                            hyperlink.NavigateUri = new Uri(entity.ExpandedUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(entity.TruncatedUrl);
                            break;
                        #endregion
                    }
                    #endregion
                    paragraph.Inlines.Add(hyperlink);
                    index += entity.Text.Length;
                }
                #region ender
                if (index < text.Length)
                {
                    paragraph.Inlines.Add(new Run
                    {
                        Text = HttpUtility.HtmlDecode(text.Substring(index, text.Length - index)),
                    });
                }
                #endregion
                #endregion
                textBox.Blocks.Add(paragraph);
            }
            #endregion
        }

        private static void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var entity = hyperlink.CommandParameter as EntityBase;
            switch (entity.EntityType)
            {
                case EntityType.UserMention:
                    var mention = entity as UserMention;
                    User user = new User
                    {
                        Id = mention.Id,
                        DisplayName = mention.DisplayName,
                    };
                    NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, user);
                    break;
                case EntityType.HashTag:
                    //TODO: to search page
                    break;
            }
        }

        private static IEnumerable<EntityBase> Select(IEnumerable<EntityBase> entityList, IEnumerable<EntityBase> parsedList)
        {
            var results =
                from entity in entityList
                from mention in parsedList
                where entity.Text.Equals(mention.Text, StringComparison.OrdinalIgnoreCase)
                select new EntityBase
                {
                    EntityType = entity.EntityType,
                    Index = mention.Index,
                    Text = entity.Text,
                    Id = entity.Id,
                    DisplayName = entity.DisplayName,
                    DisplayText = entity.DisplayText,
                    DisplayUrl = entity.DisplayUrl,
                    ExpandedUrl = entity.ExpandedUrl,
                    MediaUrl = entity.MediaUrl
                };
            return results.AsEnumerable();
        }
    }
}
