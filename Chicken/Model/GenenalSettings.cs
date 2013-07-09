namespace Chicken.Model
{
    public class GeneralSettings
    {
        public Language CurrentLanguage { get; set; }

        public APIProxy APISettings { get; set; }
    }

    public class Setting
    {
        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public bool IsInUsed { get; set; }

        public override bool Equals(object obj)
        {
            var setting = obj as Setting;
            if (setting == null)
                return false;
            if (this.Name != setting.Name)
                return false;
            return true;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }

    public class Language : Setting
    {
        public static Language[] DefaultLanguages = new Language[]
        {
            new Language{ Name ="zh-CN", DisplayName="简体中文"},
            new Language{ Name = "en-US", DisplayName="English"},
        };
    }

    public class APIProxy : Setting
    {
        public string Url { get; set; }

        public static APIProxy[] DefaultAPIs = new APIProxy[] 
        {
            new APIProxy{ Name="TWIP4", DisplayName="TWIP 4 (O mode)"},
        };
    }
}
