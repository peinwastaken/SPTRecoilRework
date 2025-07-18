using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class RecoilPosSettings
    {
        public static ConfigEntry<float> RecoilPosBackMult { get; set; } // viewmodel back
        public static ConfigEntry<float> RecoilPosIntensity { get; set; } // viewmodel recoil intensity
        public static ConfigEntry<float> RecoilPosReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> RecoilPosDamping { get; set; } // viewmodel recoil damping

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            RecoilPosBackMult = Config.Bind(formattedCategory, "Recoil Position Backwards Mult", 2f, new ConfigDescription("Multiplier for backwards hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            RecoilPosIntensity = Config.Bind(formattedCategory, "Recoil Position Intensity", 1.5f, new ConfigDescription("Hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            RecoilPosReturnSpeed = Config.Bind(formattedCategory, "Recoil Position Return Speed", 0.5f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilPosDamping = Config.Bind(formattedCategory, "Recoil Position Damping", 0.3f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
        }
    }
}
