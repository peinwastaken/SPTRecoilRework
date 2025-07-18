using BepInEx.Configuration;

namespace PeinRecoilRework.Config.Settings
{
    public class LeftStanceSettings
    {
        public static ConfigEntry<float> LeftStanceOffset { get; set; }
        public static ConfigEntry<float> LeftStanceAngle { get; set; }
        public static ConfigEntry<float> LeftStanceSpeed { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            LeftStanceOffset = Config.Bind(formattedCategory, "Left Stance Offset", 0.2f, new ConfigDescription("Offset for the left stance position.", new AcceptableValueRange<float>(0f, 0.2f), new ConfigurationManagerAttributes { Order = 900 }));
            LeftStanceAngle = Config.Bind(formattedCategory, "Left Stance Angle", 5f, new ConfigDescription("Angle for the left stance position.", new AcceptableValueRange<float>(-45f, 45f), new ConfigurationManagerAttributes { Order = 890 }));
            LeftStanceSpeed = Config.Bind(formattedCategory, "Left Stance Speed", 4f, new ConfigDescription("Speed at which the weapon moves when transitioning shoulders.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes { Order = 880 }));
        }
    }
}
