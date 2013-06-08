﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Chicken.Common;
using Chicken.Model;
using Chicken.Model.Entity;
using Chicken.Service;
using Chicken.ViewModel;
using Chicken.ViewModel.Home.Base;

namespace Chicken.Controls
{
    public class AutoRichTextBox : RichTextBox
    {
        public static DependencyProperty TweetDataProperty =
            DependencyProperty.Register("TweetData", typeof(TweetViewModel), typeof(AutoRichTextBox), new PropertyMetadata(TweetDataPropertyChanged));

        public TweetViewModel TweetData
        {
            get
            {
                return (TweetViewModel)GetValue(TweetDataProperty);
            }
            set
            {
                SetValue(TweetDataProperty, value);
            }
        }

        public static void TweetDataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as AutoRichTextBox;
            var tweet = e.NewValue as TweetViewModel;
            if (textBox == null || tweet == null)
            {
                return;
            }
            #region init
            string text = tweet.Text;
            var entities = new List<EntityBase>();
            if (tweet.Entities != null)
            {
                if (tweet.Entities.UserMentions != null && tweet.Entities.UserMentions.Count != 0)
                    foreach (var userMention in tweet.Entities.UserMentions)
                    {
                        entities.Add(userMention);
                    }
                if (tweet.Entities.Medias != null && tweet.Entities.Medias.Count != 0)
                {
                    foreach (var media in tweet.Entities.Medias)
                    {
                        entities.Add(media);
                    }
                }
                if (tweet.Entities.Urls != null && tweet.Entities.Urls.Count != 0)
                {
                    foreach (var url in tweet.Entities.Urls)
                    {
                        entities.Add(url);
                    }
                }
                if (tweet.Entities.HashTags != null && tweet.Entities.HashTags.Count != 0)
                {
                    foreach (var hashtag in tweet.Entities.HashTags)
                    {
                        entities.Add(hashtag);
                    }
                }
                entities.ForEach(entity => entity.Index = text.IndexOf(entity.Text));
                entities = entities.OrderBy(entity => entity.Index).ToList();
            }
            var paragraph = new Paragraph { TextAlignment = TextAlignment.Left };
            #endregion
            #region none
            if (entities.Count == 0)
            {
                paragraph.Inlines.Add(new Run { Text = text });
            }
            #endregion
            #region spilt
            else
            {
                int index = 0;
                foreach (var entity in entities)
                {
                    if (index < entity.Index)
                    {
                        paragraph.Inlines.Add(new Run { Text = text.Substring(index, entity.Index - index) });
                        index = entity.Index;
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
                                Command = new DelegateCommand(
                                    obj =>
                                    {
                                        var mention = obj as UserMention;
                                        var user = new UserModel
                                        {
                                            Id = mention.Id,
                                            ScreenName = mention.Text,
                                        };
                                        ScreenNameClick(user);
                                    }),
                            };
                            hyperlink.Inlines.Add(userMention.Text + " ");
                            paragraph.Inlines.Add(hyperlink);
                            break;
                        #endregion
                        #region media, url
                        case EntityType.Media:
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
                    paragraph.Inlines.Add(new Run { Text = text.Substring(index, text.Length - index) });
                }
            #endregion
            }
            textBox.Blocks.Add(paragraph);
        }

        private static void ScreenNameClick(UserModel user)
        {
            NavigationServiceManager.NavigateTo(Const.PageNameEnum.ProfilePage, user);
        }
    }
}