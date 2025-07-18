using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class RealRecoilSettings
    {
        public static ConfigEntry<bool> EnableRealRecoil { get; set; }
        public static ConfigEntry<float> RealRecoilVerticalMult { get; set; }
        public static ConfigEntry<float> RealRecoilHorizontalMult { get; set; }
        public static ConfigEntry<float> RealRecoilPistolVerticalMult { get; set; }
        public static ConfigEntry<float> RealRecoilPistolHorizontalMult { get; set; }
        public static ConfigEntry<float> RealRecoilDecaySpeed { get; set; }
        public static ConfigEntry<float> RealRecoilMountedMult { get; set; }
        public static ConfigEntry<float> RealRecoilAimingMult { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableRealRecoil = Config.Bind(formattedCategory, "Enable Real Recoil", true, new ConfigDescription("Enables real recoil, which moves the camera while shooting. The camera will kick upward and left/right randomly. The amount depends on your weapon's stats and the multipliers below.", null, new ConfigurationManagerAttributes { Order = 870 }));
            RealRecoilVerticalMult = Config.Bind(formattedCategory, "Real Recoil Vertical Mult", 2f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 860 }));
            RealRecoilHorizontalMult = Config.Bind(formattedCategory, "Real Recoil Horizontal Mult", 0.7f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 850 }));
            RealRecoilPistolVerticalMult = Config.Bind(formattedCategory, "Pistol Real Recoil Vertical Mult", 0.2f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 840 }));
            RealRecoilPistolHorizontalMult = Config.Bind(formattedCategory, "Pistol Real Recoil Horizontal Mult", 0.1f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 830 }));
            RealRecoilDecaySpeed = Config.Bind(formattedCategory, "Real Recoil Decay Speed", 20f, new ConfigDescription("Real recoil decay speed.", null, new ConfigurationManagerAttributes { Order = 820 }));
            RealRecoilMountedMult = Config.Bind(formattedCategory, "Real Recoil Mounted Multiplier", 0.5f, new ConfigDescription("Changes the amount of recoil while mounted or using bipods.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilAimingMult = Config.Bind(formattedCategory, "Real Recoil Aiming Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while aiming.", null, new ConfigurationManagerAttributes { Order = 800 }));
        }
    }
}
