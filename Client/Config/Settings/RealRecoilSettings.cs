using BepInEx.Configuration;
using UnityEngine;

namespace PeinRecoilRework.Config.Settings
{
    public class RealRecoilSettings
    {
        public static ConfigEntry<bool> EnableRealRecoil { get; set; }
        public static ConfigEntry<float> RealRecoilVerticalMult { get; set; } // base multiplier
        public static ConfigEntry<float> RealRecoilHorizontalMult { get; set; } // base multiplier
        public static ConfigEntry<float> RealRecoilDecaySpeed { get; set; }
        public static ConfigEntry<float> RealRecoilMountedMult { get; set; }
        public static ConfigEntry<float> RealRecoilCrouchMult { get; set; }
        public static ConfigEntry<float> RealRecoilProneMult { get; set; }
        public static ConfigEntry<float> RealRecoilAimingMult { get; set; }

        public static ConfigEntry<Vector2> RifleRealRecoilMult { get; set; }
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

            EnableRealRecoil = Config.Bind(formattedCategory, "Enable Real Recoil", true, new ConfigDescription("Enables real recoil, which moves the camera while shooting. The camera will kick upward and left/right randomly. The amount depends on your weapon's stats and the multipliers below.", null, new ConfigurationManagerAttributes { Order = 870 }));
            RealRecoilVerticalMult = Config.Bind(formattedCategory, "Real Recoil Vertical Mult", 2f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 860 }));
            RealRecoilHorizontalMult = Config.Bind(formattedCategory, "Real Recoil Horizontal Mult", 0.7f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 850 }));
            RealRecoilDecaySpeed = Config.Bind(formattedCategory, "Real Recoil Decay Speed", 20f, new ConfigDescription("Real recoil decay speed.", null, new ConfigurationManagerAttributes { Order = 820 }));
            RealRecoilMountedMult = Config.Bind(formattedCategory, "Real Recoil Mounted Multiplier", 0.5f, new ConfigDescription("Changes the amount of recoil while mounted or using bipods.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilCrouchMult = Config.Bind(formattedCategory, "Real Recoil Crouch Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while crouching.", null, new ConfigurationManagerAttributes { Order = 800 }));
            RealRecoilProneMult = Config.Bind(formattedCategory, "Real Recoil Prone Multiplier", 0.35f, new ConfigDescription("Changes the amount of recoil while prone.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilAimingMult = Config.Bind(formattedCategory, "Real Recoil Aiming Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while aiming.", null, new ConfigurationManagerAttributes { Order = 800 }));

            RifleRealRecoilMult = Config.Bind(formattedCategory, "Rifle Real Recoil Multiplier", new Vector2(0.7f, 1f), new ConfigDescription("Real recoil multiplier for rifles.", null, new ConfigurationManagerAttributes { Order = 790 }));
            PistolRealRecoilMult = Config.Bind(formattedCategory, "Pistol Real Recoil Multiplier", new Vector2(0.35f, 0.5f), new ConfigDescription("Real recoil multiplier for pistols.", null, new ConfigurationManagerAttributes { Order = 780 }));
            SmgRealRecoilMult = Config.Bind(formattedCategory, "SMG Real Recoil Multiplier", new Vector2(0.6f, 1f), new ConfigDescription("Real recoil multiplier for SMGs.", null, new ConfigurationManagerAttributes { Order = 770 }));
            ShotgunRealRecoilMult = Config.Bind(formattedCategory, "Shotgun Real Recoil Multiplier", new Vector2(1.1f, 1.5f), new ConfigDescription("Real recoil multiplier for shotguns.", null, new ConfigurationManagerAttributes { Order = 760 }));
            SniperRealRecoilMult = Config.Bind(formattedCategory, "Sniper Real Recoil Multiplier", new Vector2(1f, 1.3f), new ConfigDescription("Real recoil multiplier for sniper rifles.", null, new ConfigurationManagerAttributes { Order = 750 }));
            MarksmanRealRecoilMult = Config.Bind(formattedCategory, "Marksman Real Recoil Multiplier", new Vector2(0.7f, 1.3f), new ConfigDescription("Real recoil multiplier for marksman rifles.", null, new ConfigurationManagerAttributes { Order = 740 }));
            MachineGunRealRecoilMult = Config.Bind(formattedCategory, "Machine Gun Real Recoil Multiplier", new Vector2(1f, 1f), new ConfigDescription("Real recoil multiplier for machine guns.", null, new ConfigurationManagerAttributes { Order = 730 }));
            GrenadeLauncherRealRecoilMult = Config.Bind(formattedCategory, "Grenade Launcher Real Recoil Multiplier", new Vector2(0.5f, 0.5f), new ConfigDescription("Real recoil multiplier for grenade launchers.", null, new ConfigurationManagerAttributes { Order = 720 }));
        }
    }
}
