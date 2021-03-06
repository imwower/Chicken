﻿using System;
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
            DependencyProperty.Register("TweetData", typeof(ModelBase), typeof(AutoRichTextBox), new PropertyMetadata(TweetDataPropertyChanged));

        public ModelBase TweetData
        {
            get
            {
                return (ModelBase)GetValue(TweetDataProperty);
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
            if (textBox == null || e.NewValue == null)
                return;
            textBox.AddTweetData(e.NewValue);
        }

        private void AddTweetData(object data)
        {
            this.Blocks.Clear();
            string text = string.Empty;
            var paragraph = new Paragraph();
            var entities = new List<EntityBase>();
            #region tweet
            if (data is TweetBase)
            {
                var tweet = data as TweetBase;
                text = tweet.Text;
                #region add entity
                if (tweet.Entities.UserMentions != null && tweet.Entities.UserMentions.Count != 0)
                {
                    entities.AddRange(TwitterHelper.ParseUserMentions(text, tweet.Entities.UserMentions));
                }
                if (tweet.Entities.HashTags != null && tweet.Entities.HashTags.Count != 0)
                {
                    entities.AddRange(TwitterHelper.ParseHashTags(text, tweet.Entities.HashTags));
                }
                if (tweet.Entities.Urls != null && tweet.Entities.Urls.Count != 0)
                {
                    entities.AddRange(TwitterHelper.ParseUrls(text, tweet.Entities.Urls));
                }
                if (tweet.Entities.Medias != null && tweet.Entities.Medias.Count != 0)
                {
                    entities.AddRange(TwitterHelper.ParseMedias(text, tweet.Entities.Medias));
                }
                #endregion
            }
            #endregion
            #region profile
            else if (data is UserProfileDetail)
            {
                var profile = data as UserProfileDetail;
                text = profile.Text;
                var mentions = TwitterHelper.ParseUserMentions(profile.Text);
                entities.AddRange(mentions);
                var hashtags = TwitterHelper.ParseHashTags(profile.Text);
                entities.AddRange(hashtags);
                #region url
                if (profile.UserProfileEntities != null &&
                    profile.UserProfileEntities.DescriptionEntities != null &&
                    profile.UserProfileEntities.DescriptionEntities.Urls != null)
                {
                    var parsedUrls = TwitterHelper.ParseUrls(profile.Text, profile.UserProfileEntities.DescriptionEntities.Urls);
                    entities.AddRange(parsedUrls);
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
                this.Blocks.Add(paragraph);
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
                            Text = HttpUtility.HtmlDecode(text.Substring(index, entity.Index - index)),
                        });
                        index = entity.Index;
                    }
                    #endregion
                    var hyperlink = new Hyperlink();
                    hyperlink.TextDecorations = null;
                    hyperlink.Foreground = App.PhoneAccentBrush;
                    #region entity
                    switch (entity.EntityType)
                    {
                        #region mention, hashtag
                        case EntityType.UserMention:
                        case EntityType.HashTag:
                            hyperlink.CommandParameter = entity;
                            hyperlink.Click += this.Hyperlink_Click;
                            hyperlink.Inlines.Add(entity.Text);
                            break;
                        #endregion
                        #region media, url
                        case EntityType.Media:
                            var media = entity as MediaEntity;
                            hyperlink.NavigateUri = new Uri(media.MediaUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(media.TruncatedUrl);
                            break;
                        case EntityType.Url:
                            var url = entity as UrlEntity;
                            hyperlink.NavigateUri = new Uri(url.ExpandedUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(url.TruncatedUrl);
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
                this.Blocks.Add(paragraph);
            }
            #endregion
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
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
                        ScreenName = mention.DisplayName,
                    };
                    NavigationServiceManager.NavigateTo(Const.ProfilePage, user);
                    break;
                case EntityType.HashTag:
                    var query = IsolatedStorageService.GetObject<string>(Const.SearchPage);
                    if (entity.Text != query)
                        NavigationServiceManager.NavigateTo(Const.SearchPage, entity.Text);
                    break;
            }
        }
    }
}
