using BepInEx.Configuration;

namespace SPTRecoilRework.Config.Settings
{
    public class GeneralSettings
    {
        public static ConfigEntry<bool> EnableCrankRecoil { get; set; }
        public static ConfigEntry<float> CameraSnap { get; set; }
        public static ConfigEntry<float> PistolCameraSnap { get; set; }
        public static ConfigEntry<bool> AllowLeanCameraTilt { get; set; }
        public static ConfigEntry<bool> AllowStableRecoil { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableCrankRecoil = Config.Bind(formattedCategory, "Enable Crank Recoil", true, new ConfigDescription("Toggles whether your weapon recoils toward the screen.", null, new ConfigurationManagerAttributes { Order = 950 }));
            CameraSnap = Config.Bind(formattedCategory, "Camera Snap Speed", 1f, new ConfigDescription("Speed at which the weapon's ADS position adjusts to follow the weapon's rotational recoil.", new AcceptableValueRange<float>(0f, 4f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolCameraSnap = Config.Bind(formattedCategory, "Pistol Camera Snap Speed", 0.5f, new ConfigDescription("Speed at which the weapon's ADS position adjusts to follow the weapon's rotational recoil.", new AcceptableValueRange<float>(0f, 4f), new ConfigurationManagerAttributes { Order = 940 }));
            AllowLeanCameraTilt = Config.Bind(formattedCategory, "Allow Lean Camera Tilt", false, new ConfigDescription("Toggles whether the camera rotates during leaning.", null, new ConfigurationManagerAttributes { Order = 930 }));
            AllowStableRecoil = Config.Bind(formattedCategory, "Allow Vanilla Stable Recoil", true, new ConfigDescription("Allows vanilla recoil stabilization. Affected by hand rotation recoil settings.", null, new ConfigurationManagerAttributes { Order = 920 }));
        }
    }
}
