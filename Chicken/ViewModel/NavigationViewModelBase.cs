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
using System.Windows.Navigation;

namespace Chicken.ViewModel
{
    public class NavigationViewModelBase : NotificationObject
    {
        public NavigationContext NavigationContext { get; set; }

        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {

        }
    }
}
