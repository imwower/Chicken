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
using Chicken.ViewModel;

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
            var textBox = sender as AutoRichTextBox;
            var tweet = e.NewValue as TweetBase;
            if (textBox == null || tweet == null)
            {
                return;
            }
            #region init
            string text = tweet.Text;
            var entities = new List<EntityBase>();
            if (tweet.Entities != null)
            {
                if (tweet.Entities.UserMentions != null)
                {
                    foreach (var userMention in tweet.Entities.UserMentions)
                    {
                        entities.Add(userMention);
                    }
                }
                if (tweet.Entities.Medias != null)
                {
                    foreach (var media in tweet.Entities.Medias)
                    {
                        entities.Add(media);
                    }
                }
                if (tweet.Entities.Urls != null)
                {
                    foreach (var url in tweet.Entities.Urls)
                    {
                        entities.Add(url);
                    }
                }
                if (tweet.Entities.HashTags != null)
                {
                    foreach (var hashtag in tweet.Entities.HashTags)
                    {
                        entities.Add(hashtag);
                    }
                }
                entities = entities.OrderBy(entity => entity.Index).ToList();
            }
            var paragraph = new Paragraph();
            #endregion
            #region none
            if (entities.Count == 0)
            {
                paragraph.Inlines.Add(new Run
                {
                    Text = HttpUtility.HtmlDecode(text)
                });
            }
            #endregion
            #region spilt
            else
            {
                int index = 0; int position = 0;
                foreach (var entity in entities)
                {
                    if (index < entity.Index + position)
                    {
                        paragraph.Inlines.Add(new Run
                        {
                            Text = HttpUtility.HtmlDecode(text.Substring(index, entity.Index + position - index))
                        });
                        index = entity.Index + position;
                    }
                    switch (entity.EntityType)
                    {
                        #region user mention
                        case EntityType.UserMention:
                            var userMention = entity as UserMention;
                            var hyperlink = new Hyperlink
                            {
                                TextDecorations = null,
                                CommandParameter = userMention,
                                Command = new DelegateCommand(obj => ScreenNameClick(obj)),
                            };
                            hyperlink.Inlines.Add(userMention.Text + " ");
                            paragraph.Inlines.Add(hyperlink);
                            break;
                        #endregion
                        #region media, url
                        case EntityType.Media:
                            var mediaEntity = entity as MediaEntity;
                            var medialink = new Hyperlink
                            {
                                TextDecorations = null,
                                NavigateUri = new Uri(mediaEntity.MediaUrl, UriKind.Absolute),
                                TargetName = "_blank",
                            };
                            medialink.Inlines.Add(mediaEntity.TruncatedUrl + " ");
                            paragraph.Inlines.Add(medialink);
                            break;
                        case EntityType.Url:
                            var urlEntity = entity as UrlEntity;
                            var link = new Hyperlink
                            {
                                TextDecorations = null,
                                NavigateUri = new Uri(urlEntity.ExpandedUrl, UriKind.Absolute),
                                TargetName = "_blank",
                            };
                            link.Inlines.Add(urlEntity.TruncatedUrl + " ");
                            paragraph.Inlines.Add(link);
                            #region check the position of url entity
                            if (urlEntity.Text.StartsWith("http://") && urlEntity.Text.Length > App.Configuration.MaxShortUrlLength)
                            {
                                position += urlEntity.Text.Length - App.Configuration.MaxShortUrlLength;
                            }
                            if (urlEntity.Text.StartsWith("https://") && urlEntity.Text.Length > App.Configuration.MaxShortUrlLengthHttps)
                            {
                                position += urlEntity.Text.Length - App.Configuration.MaxShortUrlLengthHttps;
                            }
                            #endregion
                            break;
                        #endregion
                        #region hashtag
                        case EntityType.HashTag:
                            var hashtag = entity as HashTag;
                            var run = new Run
                            {
                                Foreground = Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush,
                                Text = hashtag.Text + " ",
                            };
                            paragraph.Inlines.Add(run);
                            break;
                        #endregion
                    }
                    index += entity.Text.Length;
                }
                //last:
                if (index < text.Length)
                {
                    paragraph.Inlines.Add(new Run
                    {
                        Text = HttpUtility.HtmlDecode(text.Substring(index, text.Length - index)),
                    });
                }
            #endregion
            }
            textBox.Blocks.Add(paragraph);
        }

        private static void ScreenNameClick(object parameter)
        {
            var mention = parameter as UserMention;
            var user = new User
            {
                Id = mention.Id,
                DisplayName = mention.DisplayName
            };
            NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, user);
        }
    }
}
