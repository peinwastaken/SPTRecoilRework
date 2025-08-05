using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class PistolRecoilSettings
    {
        public static ConfigEntry<float> RecoilPosBackMult { get; set; }
        public static ConfigEntry<float> RecoilPosIntensity { get; set; }
        public static ConfigEntry<float> RecoilPosReturnSpeed { get; set; }
        public static ConfigEntry<float> RecoilPosDamping { get; set; }

        public static ConfigEntry<float> RecoilAngUpMult { get; set; }
        public static ConfigEntry<float> RecoilAngSideMult { get; set; }
        public static ConfigEntry<float> RecoilAngReturnSpeed { get; set; }
        public static ConfigEntry<float> RecoilAngDamping { get; set; }

        public static ConfigEntry<float> RollMultiplier { get; set; }
        public static ConfigEntry<float> RollAimingMultiplier { get; set; }
        public static ConfigEntry<float> RollTimeMultiplier { get; set; }

        public static ConfigEntry<bool> AllowDynamicAdjust { get; set; }
        public static ConfigEntry<Vector2> DynamicRangeMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicMultMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicReturnMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicDampingMinMax { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            RecoilPosBackMult = Config.Bind(formattedCategory, "Pistol Recoil Position Backwards Mult", 5f, new ConfigDescription("Multiplier for backwards recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 1000 }));
            RecoilPosIntensity = Config.Bind(formattedCategory, "Pistol Recoil Position Intensity", 1f, new ConfigDescription("Recoil position intensity. Consider it an additional multiplier.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 990 }));
            RecoilPosReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Position Return Speed", 0.5f, new ConfigDescription("Recoil position return speed.", new AcceptableValueRange<float>(0.01f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            RecoilPosDamping = Config.Bind(formattedCategory, "Pistol Recoil Position Damping", 0.3f, new ConfigDescription("Recoil position damping.", new AcceptableValueRange<float>(0.01f, 5f), new ConfigurationManagerAttributes { Order = 970 }));

            RecoilAngUpMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Up Mult", 20f, new ConfigDescription("Upwards angle recoil multiplier.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilAngSideMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Side Mult", 2f, new ConfigDescription("Sideways angle recoil multiplier.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 950 }));
            RecoilAngReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Rotation Return Speed", 35f, new ConfigDescription("Recoil angle return speed.", new AcceptableValueRange<float>(0.01f, 50f), new ConfigurationManagerAttributes { Order = 940 }));
            RecoilAngDamping = Config.Bind(formattedCategory, "Pistol Recoil Rotation Damping", 0.45f, new ConfigDescription("Recoil angle damping.", new AcceptableValueRange<float>(0.01f, 5f), new ConfigurationManagerAttributes { Order = 930 }));

            RollMultiplier = Config.Bind(formattedCategory, "Roll Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while not aiming.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 920 }));
            RollAimingMultiplier = Config.Bind(formattedCategory, "Roll Aiming Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while aiming", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 910 }));
            RollTimeMultiplier = Config.Bind(formattedCategory, "Roll Speed Multiplier", 1f, new ConfigDescription("Visual recoil roll speed multiplier", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 900 }));

            AllowDynamicAdjust = Config.Bind(formattedCategory, "Allow Dynamic Adjustments", true, new ConfigDescription("Allows dynamic adjustment of multiplier, damping and return speed values based on the equipped weapon's recoil values.", null, new ConfigurationManagerAttributes { Order = 890 }));
            DynamicRangeMinMax = Config.Bind(formattedCategory, "Dynamic Recoil Range", new Vector2(350f, 500f), new ConfigDescription("Minimum and maximum VERTICAL RECOIL values used for dynamic range. Dynamic recoil adjustments begin at X and end at Y.", null, new ConfigurationManagerAttributes { Order = 880 }));
            DynamicMultMinMax = Config.Bind(formattedCategory, "Dynamic Multiplier Range", new Vector2(1f, 1.3f), new ConfigDescription("Minimum and maximum modifier used for upwards recoil angle multiplier.", null, new ConfigurationManagerAttributes { Order = 870 }));
            DynamicReturnMinMax = Config.Bind(formattedCategory, "Dynamic Return Speed Multiplier Range", new Vector2(1f, 0.2f), new ConfigDescription("Minimum and maximum modifier used for upwards recoil angle multiplier.", null, new ConfigurationManagerAttributes { Order = 860 }));
            DynamicDampingMinMax = Config.Bind(formattedCategory, "Dynamic Damping Multiplier Range", new Vector2(1f, 1.3f), new ConfigDescription("Minimum and maximum modifier used for upwards recoil angle multiplier.", null, new ConfigurationManagerAttributes { Order = 850 }));
        }
    }
}