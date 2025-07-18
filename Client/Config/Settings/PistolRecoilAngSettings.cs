using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class PistolRecoilAngSettings
    {
        public static ConfigEntry<float> PistolRecoilAngUpMult { get; set; } // pistol viewmodel rotate up/down
        public static ConfigEntry<float> PistolRecoilAngSideMult { get; set; } // pistol viewmodel rotate left/right
        public static ConfigEntry<float> PistolRecoilAngReturnSpeed { get; set; } // pistol viewmodel recoil speed
        public static ConfigEntry<float> PistolRecoilAngDamping { get; set; } // pistol viewmodel recoil damping

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            PistolRecoilAngUpMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Up Mult", 20f, new ConfigDescription("Multiplier for pistol hand recoil angle up/down.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 970 }));
            PistolRecoilAngSideMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Side Mult", 2f, new ConfigDescription("Multiplier for pistol hand recoil left/right.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolRecoilAngReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Rotation Return Speed", 35f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolRecoilAngDamping = Config.Bind(formattedCategory, "Pistol Recoil Rotation Damping", 0.45f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));
        }
    }
}
