using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Chicken.ViewModel.Home.Base;

namespace Chicken.Controls
{
    public abstract class TemplateSelector : ContentControl
    {
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            ContentTemplate = this.SelectTemplate(this, newContent);
        }

        protected virtual DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            return null;
        }
    }

    public class MessageTemplateSelector : TemplateSelector
    {
        public DataTemplate MessageTemplate { get; set; }

        public DataTemplate MyMessageTemplate { get; set; }

        protected override DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            DirectMessageViewModel message = newValue as DirectMessageViewModel;
            return MessageTemplate;
        }
    }
}
