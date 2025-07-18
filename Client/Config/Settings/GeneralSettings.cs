using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class GeneralSettings
    {
        public static ConfigEntry<bool> EnableCrankRecoil { get; set; } // recoil backwards instead of forwards
        public static ConfigEntry<float> CameraSnap { get; set; } // camera snap speed
        public static ConfigEntry<float> PistolCameraSnap { get; set; } // pistol camera snap speed
        public static ConfigEntry<bool> AllowServerOverride { get; set; } // allow server to override recoil settings
        public static ConfigEntry<bool> AllowLeanCameraTilt { get; set; }
        public static ConfigEntry<bool> AllowStableRecoil { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableCrankRecoil = Config.Bind(formattedCategory, "Enable Crank Recoil", true, new ConfigDescription("Toggles whether your weapon recoils toward the screen.", null, new ConfigurationManagerAttributes { Order = 950 }));
            CameraSnap = Config.Bind(formattedCategory, "Camera Snap Speed", 1f, new ConfigDescription("Speed at which the camera follows the weapon's recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolCameraSnap = Config.Bind(formattedCategory, "Pistol Camera Snap Speed", 0.5f, new ConfigDescription("Speed at which the camera follows the weapon's recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            AllowServerOverride = Config.Bind(formattedCategory, "Allow Server Override", true, new ConfigDescription("Allows the server to override client-side recoil settings. Currently required for some unique weapon recoils (Deagle, Glock 18c, etc.)", null, new ConfigurationManagerAttributes { Order = 920 }));
            AllowLeanCameraTilt = Config.Bind(formattedCategory, "Allow Lean Camera Tilt", false, new ConfigDescription("Changes whether the camera rotates during leaning.", null, new ConfigurationManagerAttributes { Order = 910 }));
            AllowStableRecoil = Config.Bind(formattedCategory, "Allow Vanilla Stable Recoil", true, new ConfigDescription("Allows vanilla recoil stabilization.", null, new ConfigurationManagerAttributes { Order = 909 }));
        }
    }
}
