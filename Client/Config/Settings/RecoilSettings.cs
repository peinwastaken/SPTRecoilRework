using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class RecoilSettings
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

        public static ConfigEntry<float> HipPenaltyTacOnMult { get; set; }
        public static ConfigEntry<float> HipPenaltyTacOffMult { get; set; }

        public static ConfigEntry<bool> AllowDynamicAdjust { get; set; }
        public static ConfigEntry<Vector2> DynamicRangeMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicMultMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicReturnMinMax { get; set; }
        public static ConfigEntry<Vector2> DynamicDampingMinMax { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            RecoilPosBackMult = Config.Bind(formattedCategory, "Recoil Position Backwards Mult", 2f, new ConfigDescription("Multiplier for backwards hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 1000 }));
            RecoilPosIntensity = Config.Bind(formattedCategory, "Recoil Position Intensity", 1.5f, new ConfigDescription("Recoil intensity. Consider it an additional multiplier.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 990 }));
            RecoilPosReturnSpeed = Config.Bind(formattedCategory, "Recoil Position Return Speed", 0.5f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            RecoilPosDamping = Config.Bind(formattedCategory, "Recoil Position Damping", 0.3f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));

            RecoilAngUpMult = Config.Bind(formattedCategory, "Recoil Rotation Up Mult", 0.3f, new ConfigDescription("Multiplier for hand recoil angle up/down.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilAngSideMult = Config.Bind(formattedCategory, "Recoil Rotation Side Mult", 0.3f, new ConfigDescription("Multiplier for sideways hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            RecoilAngReturnSpeed = Config.Bind(formattedCategory, "Recoil Rotation Return Speed", 4f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));
            RecoilAngDamping = Config.Bind(formattedCategory, "Recoil Rotation Damping", 0.2f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));

            RollMultiplier = Config.Bind(formattedCategory, "Roll Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while not aiming.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 920 }));
            RollAimingMultiplier = Config.Bind(formattedCategory, "Roll Aiming Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while aiming", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 910 }));
            RollTimeMultiplier = Config.Bind(formattedCategory, "Roll Speed Multiplier", 1f, new ConfigDescription("Visual recoil roll speed multiplier", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 900 }));

            HipPenaltyTacOnMult = Config.Bind(formattedCategory, "Hipfire Penalty Mult (Laser On)", 1f, new ConfigDescription("Changes the hipfire penalty multiplier when tactical devices are turned on.", null, new ConfigurationManagerAttributes { Order = 899 }));
            HipPenaltyTacOffMult = Config.Bind(formattedCategory, "Hipfire Penalty Mult (Laser Off)", 1f, new ConfigDescription("Changes the hipfire penalty multiplier when tactical devices are turned off.", null, new ConfigurationManagerAttributes { Order = 898 }));

            AllowDynamicAdjust = Config.Bind(formattedCategory, "Allow Dynamic Adjustments", true, new ConfigDescription("Allows dynamic adjustment of multiplier, damping and return speed values based on the equipped weapon's recoil values.", null, new ConfigurationManagerAttributes { Order = 890 }));
            DynamicRangeMinMax = Config.Bind(formattedCategory, "Dynamic Recoil Range", new Vector2(600f, 900f), new ConfigDescription("Minimum and maximum HORIZONTAL RECOIL values used for dynamic range. Dynamic recoil adjustments begin at X and end at Y.", null, new ConfigurationManagerAttributes { Order = 880 }));
            DynamicMultMinMax = Config.Bind(formattedCategory, "Dynamic Multiplier Range", new Vector2(1f, 0.7f), new ConfigDescription("Minimum and maximum modifier used for backwards recoil.", null, new ConfigurationManagerAttributes { Order = 870 }));
            DynamicReturnMinMax = Config.Bind(formattedCategory, "Dynamic Return Speed Multiplier Range", new Vector2(1f, 0.7f), new ConfigDescription("Minimum and maximum modifier used for backwards recoil.", null, new ConfigurationManagerAttributes { Order = 860 }));
            DynamicDampingMinMax = Config.Bind(formattedCategory, "Dynamic Damping Multiplier Range", new Vector2(1f, 0.8f), new ConfigDescription("Minimum and maximum modifier used for backwards recoil.", null, new ConfigurationManagerAttributes { Order = 850 }));
        }
    }
}