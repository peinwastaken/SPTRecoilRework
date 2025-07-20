using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class RecoilAngSettings
    {
        public static ConfigEntry<float> RecoilAngUpMult { get; set; } // viewmodel rotate up/down
        public static ConfigEntry<float> RecoilAngSideMult { get; set; } // viewmodel rotate left/right
        public static ConfigEntry<float> RecoilAngReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> RecoilAngDamping { get; set; } // viewmodel recoil damping

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            RecoilAngUpMult = Config.Bind(formattedCategory, "Recoil Rotation Up Mult", 0.3f, new ConfigDescription("Multiplier for hand recoil angle up/down.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            RecoilAngSideMult = Config.Bind(formattedCategory, "Recoil Rotation Side Mult", 0.3f, new ConfigDescription("Multiplier for sideways hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilAngReturnSpeed = Config.Bind(formattedCategory, "Recoil Rotation Return Speed", 4f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));
            RecoilAngDamping = Config.Bind(formattedCategory, "Recoil Rotation Damping", 0.2f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 920 }));
        }
    }
}
