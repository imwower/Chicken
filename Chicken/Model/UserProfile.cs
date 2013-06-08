
namespace Chicken.Model
{
    public class UserProfile : User
    {
        public string Description { get; set; }

        public string TruncatedDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(Description) && Description.Length > 60)
                {
                    return Description.Substring(0, 60) + "...";
                }
                return Description;
            }
        }
    }

    public class UserProfileList<T> : ModelBaseList<T> where T : UserProfile
    { }
}
