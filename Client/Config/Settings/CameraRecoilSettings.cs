using BepInEx.Configuration;

namespace SPTRecoilRework.Config.Settings
{
    public class CameraRecoilSettings
    {
        public static ConfigEntry<float> CameraRecoilUpMult { get; set; }
        public static ConfigEntry<float> CameraRecoilSideMult { get; set; }
        public static ConfigEntry<float> CameraRecoilIntensity { get; set; }
        public static ConfigEntry<float> CameraRecoilReturnSpeed { get; set; }
        public static ConfigEntry<float> CameraRecoilDamping { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            CameraRecoilUpMult = Config.Bind(formattedCategory, "Camera Recoil Up Mult", -0.7f, new ConfigDescription("Multiplier for vertical camera recoil", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 1000 }));
            CameraRecoilSideMult = Config.Bind(formattedCategory, "Camera Recoil Side Mult", 1f, new ConfigDescription("Multiplier for sideways camera recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 990 }));
            CameraRecoilIntensity = Config.Bind(formattedCategory, "Camera Recoil Intensity", 2f, new ConfigDescription("Camera recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            CameraRecoilReturnSpeed = Config.Bind(formattedCategory, "Camera Recoil Return Speed", 0.6f, new ConfigDescription("Camera recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            CameraRecoilDamping = Config.Bind(formattedCategory, "Camera Recoil Damping", 0.3f, new ConfigDescription("Camera recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
        }
    }
}
