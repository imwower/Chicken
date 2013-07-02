using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;
using Chicken.Service;
using System.Windows.Media;

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

        private const string pattern = @"(?<text>{0})([^\w]|$)";

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
                #region user mention
                if (tweet.Entities.UserMentions != null)
                {
                    var mentions = tweet.Entities.UserMentions.Distinct(m => m.Id);
                    foreach (var mention in mentions)
                    {
                        var matches = Regex.Matches(text, string.Format(pattern, mention.Text), RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            var m = new UserMention
                            {
                                Index = match.Index,
                                Id = mention.Id,
                                DisplayName = mention.DisplayName,
                            };
                            entities.Add(m);
                        }
                    }
                }
                #endregion
                #region media
                if (tweet.Entities.Medias != null)
                {
                    foreach (var media in tweet.Entities.Medias)
                    {
                        var matches = Regex.Matches(text, string.Format(pattern, media.Text), RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            var m = new MediaEntity
                            {
                                Index = match.Index,
                                Text = media.Text,
                                MediaUrl = media.MediaUrl,
                                DisplayUrl = media.DisplayUrl,
                            };
                            entities.Add(m);
                        }
                    }
                }
                #endregion
                #region url
                if (tweet.Entities.Urls != null)
                {
                    foreach (var url in tweet.Entities.Urls)
                    {
                        var matches = Regex.Matches(text, string.Format(pattern, url.Text), RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            var m = new UrlEntity
                            {
                                Index = match.Index,
                                Text = url.Text,
                                DisplayUrl = url.DisplayUrl,
                                ExpandedUrl = url.ExpandedUrl,
                            };
                            entities.Add(m);
                        }
                    }
                }
                #endregion
                #region hashtag
                if (tweet.Entities.HashTags != null)
                {
                    foreach (var hashtag in tweet.Entities.HashTags)
                    {
                        var matches = Regex.Matches(text, string.Format(pattern, hashtag.Text), RegexOptions.IgnoreCase);
                        foreach (Match match in matches)
                        {
                            var m = new HashTag
                            {
                                Index = match.Index,
                                Text = hashtag.Text,
                            };
                            entities.Add(m);
                        }
                    }
                }
                #endregion
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
                    var urls = profile.UserProfileEntities.DescriptionEntities.Urls.Cast<EntityBase>();
                    entities.AddRange(urls);
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
                            var mediaEntity = entity as MediaEntity;
                            hyperlink.NavigateUri = new Uri(mediaEntity.MediaUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(mediaEntity.TruncatedUrl);
                            break;
                        case EntityType.Url:
                            var urlEntity = entity as UrlEntity;
                            hyperlink.NavigateUri = new Uri(urlEntity.ExpandedUrl, UriKind.Absolute);
                            hyperlink.TargetName = "_blank";
                            hyperlink.Inlines.Add(urlEntity.TruncatedUrl);
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
    }
}
