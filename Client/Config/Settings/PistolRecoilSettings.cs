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

            RecoilPosBackMult = Config.Bind(formattedCategory, "Pistol Recoil Position Backwards Mult", 5f, new ConfigDescription("Multiplier for backwards pistol hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            RecoilPosIntensity = Config.Bind(formattedCategory, "Pistol Recoil Position Intensity", 1f, new ConfigDescription("Recoil intensity. Consider it an additional multiplier.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilPosReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Position Return Speed", 0.5f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            RecoilPosDamping = Config.Bind(formattedCategory, "Pistol Recoil Position Damping", 0.3f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));

            RecoilAngUpMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Up Mult", 20f, new ConfigDescription("Multiplier for pistol hand recoil angle up/down.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 970 }));
            RecoilAngSideMult = Config.Bind(formattedCategory, "Pistol Recoil Rotation Side Mult", 2f, new ConfigDescription("Multiplier for pistol hand recoil left/right.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            RecoilAngReturnSpeed = Config.Bind(formattedCategory, "Pistol Recoil Rotation Return Speed", 35f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 940 }));
            RecoilAngDamping = Config.Bind(formattedCategory, "Pistol Recoil Rotation Damping", 0.45f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));

            RollMultiplier = Config.Bind(formattedCategory, "Roll Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while not aiming.", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 1000 }));
            RollAimingMultiplier = Config.Bind(formattedCategory, "Roll Aiming Multiplier", 1f, new ConfigDescription("Visual recoil roll multiplier while aiming", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 990 }));
            RollTimeMultiplier = Config.Bind(formattedCategory, "Roll Speed Multiplier", 1f, new ConfigDescription("Visual recoil roll speed multiplier", new AcceptableValueRange<float>(0.1f, 10f), new ConfigurationManagerAttributes { Order = 980 }));
        }
    }
}
