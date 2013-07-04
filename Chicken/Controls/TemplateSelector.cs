using System.Windows;
using System.Windows.Controls;
using Chicken.ViewModel.Base;

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

        public DataTemplate MessageSentByMeTemplate { get; set; }

        protected override DataTemplate SelectTemplate(DependencyObject sender, object newValue)
        {
            DirectMessageViewModel message = newValue as DirectMessageViewModel;
            if (message.IsSentByMe)
            {
                return MessageSentByMeTemplate;
            }
            return MessageTemplate;
        }
    }
}
