using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class PistolRecoilPosSettings
    {
        public static ConfigEntry<float> PistolRecoilPosBackMult { get; set; } // viewmodel back
        public static ConfigEntry<float> PistolRecoilPosIntensity { get; set; } // viewmodel recoil intensity
        public static ConfigEntry<float> PistolRecoilPosReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> PistolRecoilPosDamping { get; set; } // viewmodel recoil damping

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            PistolRecoilPosBackMult = Config.Bind(formattedCategory, "Pistol Recoil Position Backwards Mult", 5f, new ConfigDescription("Multiplier for backwards pistol hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            PistolRecoilPosIntensity = Config.Bind(formattedCategory, "Pistol Recoil Position Intensity", 1f, new ConfigDescription("Pistol hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolRecoilPosReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Position Return Speed", 0.5f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            PistolRecoilPosDamping = Config.Bind(formattedCategory, "Pistol Recoil Position Damping", 0.3f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));
        }
    }
}
