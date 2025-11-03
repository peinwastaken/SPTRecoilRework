using BepInEx.Configuration;

namespace SPTRecoilRework.Config.Settings
{
    public class DebugSettings
    {
        public static ConfigEntry<bool> EnableDebug { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableDebug = Config.Bind(formattedCategory, "Enable Debug", false, new ConfigDescription("Enables debug logging.", null, new ConfigurationManagerAttributes { Order = 1000, IsAdvanced = true }));
        }
    }
}
