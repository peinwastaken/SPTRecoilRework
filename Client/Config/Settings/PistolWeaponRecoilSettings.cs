using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class PistolWeaponRecoilSettings
    {
        public static ConfigEntry<float> Multiplier;
        public static ConfigEntry<float> AimingMultiplier;
        public static ConfigEntry<float> TimeMultiplier;

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            Multiplier = Config.Bind(formattedCategory, "Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while not aiming.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 1000 }));
            AimingMultiplier = Config.Bind(formattedCategory, "Aiming Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while aiming", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 990 }));
            TimeMultiplier = Config.Bind(formattedCategory, "Speed Multiplier", 1f, new ConfigDescription("Visual recoil roll speed multiplier", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 980 }));
        }
    }
}
