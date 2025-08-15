using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class RealRecoilSettings
    {
        public static ConfigEntry<bool> EnableRealRecoil { get; set; }
        public static ConfigEntry<bool> RealRecoil172Behavior { get; set; }
        public static ConfigEntry<float> RealRecoilVerticalMult { get; set; } // base multiplier
        public static ConfigEntry<float> RealRecoilHorizontalMult { get; set; } // base multiplier
        public static ConfigEntry<float> RealRecoilDecaySpeed { get; set; }
        public static ConfigEntry<bool> EnableRealRecoilAlternateSystem { get; set; }
        public static ConfigEntry<float> AlternateRealRecoilSpeed { get; set; }
        public static ConfigEntry<float> AlternateRealRecoilStiffness { get; set; }
        public static ConfigEntry<float> AlternateRealRecoilDamping { get; set; }
        public static ConfigEntry<float> RealRecoilMountedMult { get; set; }
        public static ConfigEntry<float> RealRecoilCrouchMult { get; set; }
        public static ConfigEntry<float> RealRecoilProneMult { get; set; }
        public static ConfigEntry<float> RealRecoilAimingMult { get; set; }

        public static ConfigEntry<bool> EnableRealRecoilPerWeaponMults { get; set; }
        public static ConfigEntry<Vector2> RifleRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> CarbineRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> PistolRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> SmgRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> ShotgunRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> SniperRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> MarksmanRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> MachineGunRealRecoilMult { get; set; }
        public static ConfigEntry<Vector2> GrenadeLauncherRealRecoilMult { get; set; }

        public static void Bind(ConfigFile Config, int order, string category)
        {
            string formattedCategory = Category.Format(order, category);

            EnableRealRecoil = Config.Bind(formattedCategory, "Enable Real Recoil", true, new ConfigDescription("Enables real recoil, which simulates recoil by physically rotating the camera. The amount depends on your weapon's recoil stats and the multipliers below.", null, new ConfigurationManagerAttributes { Order = 870 }));

            RealRecoil172Behavior = Config.Bind(formattedCategory, "Use 1.7.2 Recoil Values", false, new ConfigDescription("(TEMPORARY) Makes real recoil use 1.7.2 values (for the most part) for calculating real recoil force amounts. Requires config changes. Baseline settings: Vertical mult: 4.0, Horizontal mult: 0.6, All category mults: 1.0", null, new ConfigurationManagerAttributes { Order = 865 }));
            
            RealRecoilVerticalMult = Config.Bind(formattedCategory, "Real Recoil Vertical Mult", 1.2f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 860 }));
            RealRecoilHorizontalMult = Config.Bind(formattedCategory, "Real Recoil Horizontal Mult", 0.2f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 850 }));
            RealRecoilDecaySpeed = Config.Bind(formattedCategory, "Real Recoil Decay Speed", 20f, new ConfigDescription("Real recoil decay speed.", null, new ConfigurationManagerAttributes { Order = 820 }));
            EnableRealRecoilAlternateSystem = Config.Bind(formattedCategory, "Enable Alternate Real Recoil", false, new ConfigDescription("Enables an alternate version of real recoil that uses a spring based system and has auto-compensation.", null, new ConfigurationManagerAttributes { Order = 819 }));
            AlternateRealRecoilSpeed = Config.Bind(formattedCategory, "Alternate Real Recoil Spring Speed", 6f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 818 }));
            AlternateRealRecoilStiffness = Config.Bind(formattedCategory, "Alternate Real Recoil Spring Stiffness", 3f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 817 }));
            AlternateRealRecoilDamping = Config.Bind(formattedCategory, "Alternate Real Recoil Spring Damping", 0.02f, new ConfigDescription("", null, new ConfigurationManagerAttributes { Order = 816 }));
            RealRecoilMountedMult = Config.Bind(formattedCategory, "Real Recoil Mounted Multiplier", 0.5f, new ConfigDescription("Changes the amount of recoil while mounted or using a bipod.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilCrouchMult = Config.Bind(formattedCategory, "Real Recoil Crouch Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while crouching.", null, new ConfigurationManagerAttributes { Order = 800 }));
            RealRecoilProneMult = Config.Bind(formattedCategory, "Real Recoil Prone Multiplier", 0.35f, new ConfigDescription("Changes the amount of recoil while prone.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilAimingMult = Config.Bind(formattedCategory, "Real Recoil Aiming Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while aiming.", null, new ConfigurationManagerAttributes { Order = 800 }));

            EnableRealRecoilPerWeaponMults = Config.Bind(formattedCategory, "Enable Weapon Class Multipliers", true, new ConfigDescription("Enables class specific real recoil multipliers. Recommended to keep enabled since pistols have insane recoil values.", null, new ConfigurationManagerAttributes { Order = 795 }));
            RifleRealRecoilMult = Config.Bind(formattedCategory, "Rifle Real Recoil Multiplier", new Vector2(0.7f, 1f), new ConfigDescription("Real recoil multiplier for rifles.", null, new ConfigurationManagerAttributes { Order = 790 }));
            CarbineRealRecoilMult = Config.Bind(formattedCategory, "Carbine Real Recoil Multiplier", new Vector2(0.7f, 1.3f), new ConfigDescription("Real recoil multiplier for carbines.", null, new ConfigurationManagerAttributes { Order = 780 }));
            PistolRealRecoilMult = Config.Bind(formattedCategory, "Pistol Real Recoil Multiplier", new Vector2(0.35f, 0.5f), new ConfigDescription("Real recoil multiplier for pistols.", null, new ConfigurationManagerAttributes { Order = 780 }));
            SmgRealRecoilMult = Config.Bind(formattedCategory, "SMG Real Recoil Multiplier", new Vector2(0.6f, 1f), new ConfigDescription("Real recoil multiplier for SMGs.", null, new ConfigurationManagerAttributes { Order = 770 }));
            ShotgunRealRecoilMult = Config.Bind(formattedCategory, "Shotgun Real Recoil Multiplier", new Vector2(1.1f, 1.5f), new ConfigDescription("Real recoil multiplier for shotguns.", null, new ConfigurationManagerAttributes { Order = 760 }));
            SniperRealRecoilMult = Config.Bind(formattedCategory, "Sniper Real Recoil Multiplier", new Vector2(1f, 1.3f), new ConfigDescription("Real recoil multiplier for sniper rifles.", null, new ConfigurationManagerAttributes { Order = 750 }));
            MarksmanRealRecoilMult = Config.Bind(formattedCategory, "Marksman Real Recoil Multiplier", new Vector2(0.7f, 1.3f), new ConfigDescription("Real recoil multiplier for marksman rifles.", null, new ConfigurationManagerAttributes { Order = 740 }));
            MachineGunRealRecoilMult = Config.Bind(formattedCategory, "Machine Gun Real Recoil Multiplier", new Vector2(1f, 1f), new ConfigDescription("Real recoil multiplier for machine guns.", null, new ConfigurationManagerAttributes { Order = 730 }));
            GrenadeLauncherRealRecoilMult = Config.Bind(formattedCategory, "Grenade Launcher Real Recoil Multiplier", new Vector2(1f, 2f), new ConfigDescription("Real recoil multiplier for grenade launchers.", null, new ConfigurationManagerAttributes { Order = 720 }));
        }
    }
}
