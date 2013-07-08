namespace Chicken.ViewModel
{
    public class AppViewModel
    {
        public LanguageHelper Lang { get; private set; }

        public AppViewModel()
        {
            Lang = new LanguageHelper();
        }
    }
}
