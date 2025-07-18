using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class AdditionalCameraRecoilSettings
    {
        public static ConfigEntry<bool> EnableAdditionalCameraRecoil { get; set; }
        // public static ConfigEntry<bool> UseTrueRecoil { get; set; }

        public static ConfigEntry<bool> EnableSlowSpring { get; set; }
        public static ConfigEntry<float> SlowSpringSpeed { get; set; }
        public static ConfigEntry<float> SlowSpringStiffness { get; set; }
        public static ConfigEntry<float> SlowSpringDamping { get; set; }
        public static ConfigEntry<Vector2> SlowSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> SlowSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> SlowSpringAngleMinMax { get; set; }

        public static ConfigEntry<bool> EnableFastSpring { get; set; }
        public static ConfigEntry<float> FastSpringSpeed { get; set; }
        public static ConfigEntry<float> FastSpringStiffness { get; set; }
        public static ConfigEntry<float> FastSpringDamping { get; set; }
        public static ConfigEntry<Vector2> FastSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> FastSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> FastSpringAngleMinMax { get; set; }

        public static ConfigEntry<bool> EnableCameraSpring { get; set; }
        public static ConfigEntry<float> CameraSpringSpeed { get; set; }
        public static ConfigEntry<float> CameraSpringStiffness { get; set; }
        public static ConfigEntry<float> CameraSpringDamping { get; set; }
        public static ConfigEntry<Vector3> CameraSpringDirection { get; set; }
        public static ConfigEntry<Vector2> CameraSpringHorizontalMinMax { get; set; }
        public static ConfigEntry<Vector2> CameraSpringVerticalMinMax { get; set; }
        public static ConfigEntry<Vector2> CameraSpringRotationMinMax { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableAdditionalCameraRecoil = Config.Bind(formattedCategory, "Additional Camera Recoil", true, new ConfigDescription("Enables additional camera recoil.", null, new ConfigurationManagerAttributes { Order = 1000 }));
            // UseTrueRecoil = Config.Bind(formattedCategory, "Use Real Recoil Values", false, new ConfigDescription("Uses real recoil:tm: values to determine how much additional camera recoil to apply.", null, new ConfigurationManagerAttributes { Order = 990 }));

            EnableSlowSpring = Config.Bind(formattedCategory, "Enable Slow Spring", false, new ConfigDescription("Enables slow additional camera recoil.", null, new ConfigurationManagerAttributes { Order = 980 }));
            SlowSpringSpeed = Config.Bind(formattedCategory, "Slow Spring Speed", 18f, new ConfigDescription("Speed of the slow spring", null, new ConfigurationManagerAttributes { Order = 970 }));
            SlowSpringStiffness = Config.Bind(formattedCategory, "Slow Spring Stiffness", 45f, new ConfigDescription("Stiffness of the slow spring", null, new ConfigurationManagerAttributes { Order = 960 }));
            SlowSpringDamping = Config.Bind(formattedCategory, "Slow Spring Damping", 0.001f, new ConfigDescription("Damping of the slow spring", null, new ConfigurationManagerAttributes { Order = 950 }));
            SlowSpringAngleMinMax = Config.Bind(formattedCategory, "Fast Spring Direction Angle", new Vector2(30f, 150f), new ConfigDescription("Minimum/maximum angle for slow recoil spring. X = minimum, Y = maximum, 0 - right, 90 - up, 180 - left, 270 - down.", null, new ConfigurationManagerAttributes { Order = 940 }));
            SlowSpringHorizontalMinMax = Config.Bind(formattedCategory, "Slow Spring Horizontal Mult Min/Max", new Vector2(-0.15f, 0.15f), new ConfigDescription("Minimum/maximum horizontal force multiplier for slow recoil spring.", null, new ConfigurationManagerAttributes { Order = 930 }));
            SlowSpringVerticalMinMax = Config.Bind(formattedCategory, "Slow Spring Vertical Mult Min/Max", new Vector2(0.3f, 0.3f), new ConfigDescription("Minimum/maximum vertical force multiplier for slow recoil spring.", null, new ConfigurationManagerAttributes { Order = 920 }));
            
            EnableFastSpring = Config.Bind(formattedCategory, "Enable Fast Spring", true, new ConfigDescription("Enables fast additional camera recoil", null, new ConfigurationManagerAttributes { Order = 910 }));
            FastSpringSpeed = Config.Bind(formattedCategory, "Fast Spring Speed", 24f, new ConfigDescription("Speed of the fast spring", null, new ConfigurationManagerAttributes { Order = 900 }));
            FastSpringStiffness = Config.Bind(formattedCategory, "Fast Spring Stiffness", 6f, new ConfigDescription("Stiffness of the fast spring", null, new ConfigurationManagerAttributes { Order = 890 }));
            FastSpringDamping = Config.Bind(formattedCategory, "Fast Spring Damping", 0.1f, new ConfigDescription("Damping of the fast spring", null, new ConfigurationManagerAttributes { Order = 880 }));
            FastSpringAngleMinMax = Config.Bind(formattedCategory, "Fast Spring Direction Angle", new Vector2(30f, 150f), new ConfigDescription("Minimum/maximum angle for fast recoil spring. X = minimum, Y = maximum, 0 - right, 90 - up, 180 - left, 270 - down.", null, new ConfigurationManagerAttributes { Order = 870 }));
            FastSpringHorizontalMinMax = Config.Bind(formattedCategory, "Fast Horizontal Mult Min/Max", new Vector2(0.15f, 0.15f), new ConfigDescription("Minimum/maximum horizontal force multiplier for fast recoil spring.", null, new ConfigurationManagerAttributes { Order = 860 }));
            FastSpringVerticalMinMax = Config.Bind(formattedCategory, "Fast Vertical Mult Min/Max", new Vector2(0.1f, 0.3f), new ConfigDescription("Minimum/maximum vertical force multiplier for fast recoil spring.", null, new ConfigurationManagerAttributes { Order = 850 }));
            
            EnableCameraSpring = Config.Bind(formattedCategory, "Enable Camera Spring", true, new ConfigDescription("Enables camera spring", null, new ConfigurationManagerAttributes { Order = 840 }));
            CameraSpringSpeed = Config.Bind(formattedCategory, "Camera Spring Speed", 20f, new ConfigDescription("Speed of the camera spring", null, new ConfigurationManagerAttributes { Order = 830 }));
            CameraSpringStiffness = Config.Bind(formattedCategory, "Camera Spring Stiffness", 15f, new ConfigDescription("Stiffness of the camera spring", null, new ConfigurationManagerAttributes { Order = 820 }));
            CameraSpringDamping = Config.Bind(formattedCategory, "Camera Spring Damping", 0.1f, new ConfigDescription("Damping of the camera spring", null, new ConfigurationManagerAttributes { Order = 810 }));
            CameraSpringDirection = Config.Bind(formattedCategory, "Camera Spring Direction", new Vector3(2f, 0f, 5f), new ConfigDescription("Direction of the camera spring. X = pitch, Y = yaw, Z = roll", null, new ConfigurationManagerAttributes { Order = 800 }));
            CameraSpringHorizontalMinMax = Config.Bind(formattedCategory, "Camera Spring Horizontal Min/Max", new Vector2(-1f, 0f), new ConfigDescription("Minimum/maximum horizontal force for camera spring.", null, new ConfigurationManagerAttributes { Order = 790 }));
            CameraSpringVerticalMinMax = Config.Bind(formattedCategory, "Camera Spring Vertical Min/Max", new Vector2(0f, 0f), new ConfigDescription("Minimum/maximum vertical force for camera spring.", null, new ConfigurationManagerAttributes { Order = 780 }));
            CameraSpringRotationMinMax = Config.Bind(formattedCategory, "Camera Spring Rotation Min/Max", new Vector2(1f, 1.5f), new ConfigDescription("Minimum/maximum rotation for camera spring.", null, new ConfigurationManagerAttributes { Order = 770 }));
        }
    }
}
