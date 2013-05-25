using System.Collections.Generic;

namespace Chicken.Service.Interface
{
    public interface INavigationService
    {
        void ChangeSelectedIndex(int selectedIndex, IDictionary<string, object> parameters = null);
    }
}
