using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class AdditionalCameraRecoilSettings
    {
        public static ConfigEntry<bool> EnableAdditionalCameraRecoil { get; set; } 
        public static ConfigEntry<float> AdditionalRecoilIronsMult { get; set; }
        public static ConfigEntry<float> AdditionalRecoilOpticsMult { get; set; }

        public static ConfigEntry<bool> EnableSlowSpring { get; set; }
        public static ConfigEntry<float> SlowSpringSpeed { get; set; }
        public static ConfigEntry<float> SlowSpringStiffness { get; set; }
        public static ConfigEntry<float> SlowSpringDamping { get; set; }
        public static ConfigEntry<Vector2> SlowSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> SlowSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> SlowSpringAngleMinMax { get; set; }
        public static ConfigEntry<bool> SlowSpringUseRealRecoilDir { get; set; }
        public static ConfigEntry<bool> SlowSpringRealRecoilDirNormalize { get; set; }
        public static ConfigEntry<Vector2> SlowSpringRealRecoilVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> SlowSpringRealRecoilHorizontalMinMax { get; set; }

        public static ConfigEntry<bool> EnableFastSpring { get; set; }
        public static ConfigEntry<float> FastSpringSpeed { get; set; }
        public static ConfigEntry<float> FastSpringStiffness { get; set; }
        public static ConfigEntry<float> FastSpringDamping { get; set; }
        public static ConfigEntry<Vector2> FastSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> FastSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> FastSpringAngleMinMax { get; set; }
        public static ConfigEntry<bool> FastSpringUseRealRecoilDir { get; set; }
        public static ConfigEntry<bool> FastSpringRealRecoilDirNormalize { get; set; }
        public static ConfigEntry<Vector2> FastSpringRealRecoilVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> FastSpringRealRecoilHorizontalMinMax { get; set; }

        public static ConfigEntry<bool> EnableCameraSpring { get; set; }
        public static ConfigEntry<float> CameraSpringSpeed { get; set; }
        public static ConfigEntry<float> CameraSpringStiffness { get; set; }
        public static ConfigEntry<float> CameraSpringDamping { get; set; }
        public static ConfigEntry<Vector3> CameraSpringDirection { get; set; }
        public static ConfigEntry<Vector2> CameraSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> CameraSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> CameraSpringRotationMinMax { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableAdditionalCameraRecoil = Config.Bind(formattedCategory, "Additional Camera Recoil", true, new ConfigDescription("Enables additional camera recoil.", null, new ConfigurationManagerAttributes { Order = 1000 }));
            AdditionalRecoilIronsMult = Config.Bind(formattedCategory, "Additional Recoil Ironsight Multiplier", 0f, new ConfigDescription("Multiplier for additional recoil when using ironsights.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 990 }));
            AdditionalRecoilOpticsMult = Config.Bind(formattedCategory, "Additional Recoil Optics Multiplier", 1f, new ConfigDescription("Multiplier for additional recoil when using optics.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 985 }));

            EnableSlowSpring = Config.Bind(formattedCategory, "Enable Slow Spring", false, new ConfigDescription("Enables slow additional camera recoil. Just in case you need an additional spring to control.", null, new ConfigurationManagerAttributes { Order = 980 })); 
            SlowSpringSpeed = Config.Bind(formattedCategory, "Slow Spring Speed", 18f, new ConfigDescription("Speed of the slow spring", null, new ConfigurationManagerAttributes { Order = 970 }));
            SlowSpringStiffness = Config.Bind(formattedCategory, "Slow Spring Stiffness", 45f, new ConfigDescription("Stiffness of the slow spring", null, new ConfigurationManagerAttributes { Order = 960 }));
            SlowSpringDamping = Config.Bind(formattedCategory, "Slow Spring Damping", 0.001f, new ConfigDescription("Damping of the slow spring", null, new ConfigurationManagerAttributes { Order = 950 }));
            SlowSpringAngleMinMax = Config.Bind(formattedCategory, "Slow Spring Direction Angle", new Vector2(30f, 150f), new ConfigDescription("Minimum/maximum angle for slow recoil spring. X = minimum, Y = maximum, 0 - right, 90 - up, 180 - left, 270 - down.", null, new ConfigurationManagerAttributes { Order = 940 }));
            SlowSpringVerticalMinMax = Config.Bind(formattedCategory, "Slow Spring Vertical Mult Min/Max", new Vector2(0.3f, 0.3f), new ConfigDescription("Minimum/maximum vertical force multiplier for slow recoil spring.", null, new ConfigurationManagerAttributes { Order = 930 }));
            SlowSpringHorizontalMinMax = Config.Bind(formattedCategory, "Slow Spring Horizontal Mult Min/Max", new Vector2(-0.15f, 0.15f), new ConfigDescription("Minimum/maximum horizontal force multiplier for slow recoil spring.", null, new ConfigurationManagerAttributes { Order = 920 }));
            SlowSpringUseRealRecoilDir = Config.Bind(formattedCategory, "Slow Spring Use Real Recoil", false, new ConfigDescription("Makes slow spring use real recoil direction for impulse direction.", null, new ConfigurationManagerAttributes { Order = 919 }));
            SlowSpringRealRecoilDirNormalize = Config.Bind(formattedCategory, "Slow Spring Real Recoil Direction Normalize", false, new ConfigDescription("Normalizes real recoil force for slow spring which makes every weapon apply the same amount of force to the spring.", null, new ConfigurationManagerAttributes { Order = 918 }));
            SlowSpringRealRecoilVerticalMinMax = Config.Bind(formattedCategory, "Slow Spring Real Recoil Vertical Min/Max", new Vector2(0.1f, 0.1f), new ConfigDescription("Multiplier range for vertical recoil. X = minimum, Y = maximum.", null, new ConfigurationManagerAttributes { Order = 917 }));
            SlowSpringRealRecoilHorizontalMinMax = Config.Bind(formattedCategory, "Slow Spring Real Recoil Horizontal Min/Max", new Vector2(0.1f, 0.1f), new ConfigDescription("Multiplier range for horizontal recoil. X = minimum, Y = maximum.", null, new ConfigurationManagerAttributes { Order = 916 }));

            EnableFastSpring = Config.Bind(formattedCategory, "Enable Fast Spring", true, new ConfigDescription("Enables fast additional camera recoil.", null, new ConfigurationManagerAttributes { Order = 910 }));
            FastSpringSpeed = Config.Bind(formattedCategory, "Fast Spring Speed", 24f, new ConfigDescription("Speed of the fast spring", null, new ConfigurationManagerAttributes { Order = 900 }));
            FastSpringStiffness = Config.Bind(formattedCategory, "Fast Spring Stiffness", 6f, new ConfigDescription("Stiffness of the fast spring", null, new ConfigurationManagerAttributes { Order = 890 }));
            FastSpringDamping = Config.Bind(formattedCategory, "Fast Spring Damping", 0.1f, new ConfigDescription("Damping of the fast spring", null, new ConfigurationManagerAttributes { Order = 880 }));
            FastSpringAngleMinMax = Config.Bind(formattedCategory, "Fast Spring Direction Angle", new Vector2(30f, 150f), new ConfigDescription("Minimum/maximum angle for fast recoil spring. X = minimum, Y = maximum, 0 - right, 90 - up, 180 - left, 270 - down.", null, new ConfigurationManagerAttributes { Order = 870 }));
            FastSpringVerticalMinMax = Config.Bind(formattedCategory, "Fast Vertical Mult Min/Max", new Vector2(0.05f, 0.2f), new ConfigDescription("Minimum/maximum vertical force multiplier for fast recoil spring.", null, new ConfigurationManagerAttributes { Order = 860 }));
            FastSpringHorizontalMinMax = Config.Bind(formattedCategory, "Fast Horizontal Mult Min/Max", new Vector2(-0.1f, 0.1f), new ConfigDescription("Minimum/maximum horizontal force multiplier for fast recoil spring.", null, new ConfigurationManagerAttributes { Order = 850 }));
            FastSpringUseRealRecoilDir = Config.Bind(formattedCategory, "Fast Spring Use Real Recoil", true, new ConfigDescription("Makes fast spring use real recoil direction for impulse direction.", null, new ConfigurationManagerAttributes { Order = 849 }));
            FastSpringRealRecoilDirNormalize = Config.Bind(formattedCategory, "Fast Spring Real Recoil Direction Normalize", false, new ConfigDescription("Normalizes real recoil force for fast spring which makes every weapon apply the same amount of force to the spring.", null, new ConfigurationManagerAttributes { Order = 848 }));
            FastSpringRealRecoilVerticalMinMax = Config.Bind(formattedCategory, "Fast Spring Real Recoil Vertical Min/Max", new Vector2(0.5f, 0.8f), new ConfigDescription("Multiplier range for vertical recoil. X = minimum, Y = maximum.", null, new ConfigurationManagerAttributes { Order = 847 }));
            FastSpringRealRecoilHorizontalMinMax = Config.Bind(formattedCategory, "Fast Spring Real Recoil Horizontal Min/Max", new Vector2(0.3f, 1f), new ConfigDescription("Multiplier range for horizontal recoil. X = minimum, Y = maximum.", null, new ConfigurationManagerAttributes { Order = 846 }));

            EnableCameraSpring = Config.Bind(formattedCategory, "Enable Camera Spring", true, new ConfigDescription("Enables camera spring", null, new ConfigurationManagerAttributes { Order = 840 }));
            CameraSpringSpeed = Config.Bind(formattedCategory, "Camera Spring Speed", 20f, new ConfigDescription("Speed of the camera spring", null, new ConfigurationManagerAttributes { Order = 830 }));
            CameraSpringStiffness = Config.Bind(formattedCategory, "Camera Spring Stiffness", 15f, new ConfigDescription("Stiffness of the camera spring", null, new ConfigurationManagerAttributes { Order = 820 }));
            CameraSpringDamping = Config.Bind(formattedCategory, "Camera Spring Damping", 0.1f, new ConfigDescription("Damping of the camera spring", null, new ConfigurationManagerAttributes { Order = 810 }));
            CameraSpringDirection = Config.Bind(formattedCategory, "Camera Spring Direction", new Vector3(2f, 0f, 5f), new ConfigDescription("Direction of the camera spring. X = pitch, Y = yaw, Z = roll", null, new ConfigurationManagerAttributes { Order = 800 }));
            CameraSpringVerticalMinMax = Config.Bind(formattedCategory, "Camera Spring Vertical Min/Max", new Vector2(0f, 0f), new ConfigDescription("Minimum/maximum vertical force for camera spring.", null, new ConfigurationManagerAttributes { Order = 790 }));
            CameraSpringHorizontalMinMax = Config.Bind(formattedCategory, "Camera Spring Horizontal Min/Max", new Vector2(-1f, 0f), new ConfigDescription("Minimum/maximum horizontal force for camera spring.", null, new ConfigurationManagerAttributes { Order = 780 }));
            CameraSpringRotationMinMax = Config.Bind(formattedCategory, "Camera Spring Rotation Min/Max", new Vector2(1f, 1.5f), new ConfigDescription("Minimum/maximum rotation for camera spring.", null, new ConfigurationManagerAttributes { Order = 770 }));
        }
    }
}
