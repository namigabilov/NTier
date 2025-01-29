namespace WebApi.Configurations
{
    public static class LocalizationConfig
    {
        public static readonly List<string> supportedLanguages = new List<string>
        {
            "az",
            "tr"
        };

        public static string[] GetSupportedCultures()
        {
            return supportedLanguages.ToArray();
        }

        public static string GetDefaultCulture()
        {
            return supportedLanguages[0];
        }
    }
}