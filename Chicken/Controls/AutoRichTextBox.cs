using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
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

        private const string paragraphTemplate = @"<Paragraph xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                    {0}
                </Paragraph>";

        private const string hyperlinkTemplate = @"<Hyperlink TextDecorations=""None"" Foreground=""{{StaticResource PhoneAccentBrush}}"" NavigateUri=""{0}"" TargetName=""{1}"">
                    {2}
            </Hyperlink>";

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
            var entities = new List<EntityBase>();
            #region
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
            }
            #endregion
            #endregion
            #region none
            if (entities.Count == 0)
            {
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(new Run
                {
                    Text = HttpUtility.HtmlDecode(tweet.Text)
                });
                textBox.Blocks.Add(paragraph);
            }
            #endregion
            #region add
            else
            {
                entities = entities.Distinct(n => n.Text).ToList();
                string xaml = tweet.Text;
                string hyperlink = string.Empty;
                #region replace
                foreach (var entity in entities)
                {
                    switch (entity.EntityType)
                    {
                        case EntityType.UserMention:
                            var mention = entity as UserMention;
                            hyperlink = string.Format(hyperlinkTemplate, mention.Id, mention.EntityType.ToString(), mention.Text);
                            break;
                        case EntityType.HashTag:
                            var tag = entity as HashTag;
                            hyperlink = string.Format(hyperlinkTemplate, tag.Text, tag.EntityType.ToString(), tag.Text);
                            break;
                        case EntityType.Media:
                            var media = entity as MediaEntity;
                            hyperlink = string.Format(hyperlinkTemplate, media.MediaUrl, "_blank", media.TruncatedUrl);
                            break;
                        case EntityType.Url:
                            var url = entity as UrlEntity;
                            hyperlink = string.Format(hyperlinkTemplate, url.ExpandedUrl, "_blank", url.TruncatedUrl);
                            break;
                        default:
                            break;
                    }
                    xaml = xaml.Replace(entity.Text, hyperlink, StringComparison.OrdinalIgnoreCase);
                }
                #endregion
                //replace & charactor
                xaml = xaml.Replace("&", "&amp;");
                string graph = string.Format(paragraphTemplate, xaml);
                Paragraph paragraph = (Paragraph)XamlReader.Load(graph);
                foreach (var inline in paragraph.Inlines)
                {
                    if (inline is Run)
                    {
                        var run = inline as Run;
                        run.Text = HttpUtility.HtmlDecode(run.Text);
                    }
                    else if (inline is Hyperlink)
                    {
                        var link = inline as Hyperlink;
                        if (link.TargetName != "_blank")
                        {
                            link.CommandParameter = link.NavigateUri.OriginalString;
                            link.NavigateUri = null;
                            link.Click += Hyperlink_Click;
                        }
                    }
                }
                textBox.Blocks.Add(paragraph);
            }
            #endregion
        }

        private static void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            EntityType type = (EntityType)Enum.Parse(typeof(EntityType), hyperlink.TargetName, true);
            switch (type)
            {
                case EntityType.UserMention:
                    User user = new User();
                    user.DisplayName = (hyperlink.Inlines[0] as Run).Text.Replace("@", "");
                    user.Id = hyperlink.CommandParameter == null ? string.Empty : hyperlink.CommandParameter as string;
                    NavigationServiceManager.NavigateTo(PageNameEnum.ProfilePage, user);
                    break;
                case EntityType.HashTag:
                    //TODO: to search page
                    break;
            }
        }
    }
}
