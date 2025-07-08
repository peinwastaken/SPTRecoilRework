using BepInEx;
using BepInEx.Configuration;
using PeinRecoilRework.Config;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using PeinRecoilRework.Patches;
using System.Collections.Generic;

namespace PeinRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "PeinRecoilRework", "1.4.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static List<WeaponRecoilData> WeaponRecoils { get; set; }

        // general settings
        public static ConfigEntry<bool> EnableCrankRecoil { get; set; } // recoil backwards instead of forwards
        public static ConfigEntry<float> CameraSnap { get; set; } // camera snap speed
        public static ConfigEntry<float> PistolCameraSnap { get; set; } // pistol camera snap speed
        public static ConfigEntry<bool> AllowServerOverride { get; set; } // allow server to override recoil settings
        public static ConfigEntry<bool> AllowLeanCameraTilt { get; set; }
        public static ConfigEntry<bool> AllowStableRecoil { get; set; }

        // left stance
        public static ConfigEntry<float> LeftStanceOffset { get; set; } // left stance offset
        public static ConfigEntry<float> LeftStanceAngle { get; set; } // left stance angle
        public static ConfigEntry<float> LeftStanceSpeed { get; set; } // left stance speed

        // real recoil
        public static ConfigEntry<bool> EnableRealRecoil { get; set; }
        public static ConfigEntry<float> RealRecoilVerticalMult { get; set; }
        public static ConfigEntry<float> RealRecoilHorizontalMult { get; set; }
        public static ConfigEntry<float> RealRecoilPistolVerticalMult { get; set; }
        public static ConfigEntry<float> RealRecoilPistolHorizontalMult { get; set; }
        public static ConfigEntry<float> RealRecoilDecaySpeed { get; set; }
        public static ConfigEntry<float> RealRecoilMountedMult { get; set; }
        public static ConfigEntry<float> RealRecoilAimingMult { get; set; }

        // cam recoil
        public static ConfigEntry<float> CameraRecoilUpMult { get; set; } // camera recoil up/down
        public static ConfigEntry<float> CameraRecoilSideMult { get; set; } // camera recoil left/right
        public static ConfigEntry<float> CameraRecoilIntensity { get; set; } // camera recoil intensity
        public static ConfigEntry<float> CameraRecoilReturnSpeed { get; set; } // camera recoil speed
        public static ConfigEntry<float> CameraRecoilDamping { get; set; } // camera recoil damping

        // hand pos recoil
        public static ConfigEntry<float> HandRecoilPosBackMult { get; set; } // viewmodel back
        public static ConfigEntry<float> HandRecoilPosIntensity { get; set; } // viewmodel recoil intensity
        public static ConfigEntry<float> HandRecoilPosReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> HandRecoilPosDamping { get; set; } // viewmodel recoil damping

        // pistol pos recoil
        public static ConfigEntry<float> PistolHandRecoilPosBackMult { get; set; } // viewmodel back
        public static ConfigEntry<float> PistolHandRecoilPosIntensity { get; set; } // pistol viewmodel recoil intensity
        public static ConfigEntry<float> PistolHandRecoilPosReturnSpeed { get; set; } // pistol viewmodel recoil speed
        public static ConfigEntry<float> PistolHandRecoilPosDamping { get; set; } // pistol viewmodel recoil damping

        // hand ang recoil
        public static ConfigEntry<float> HandRecoilAngUpMult { get; set; } // viewmodel rotate up/down
        public static ConfigEntry<float> HandRecoilAngSideMult { get; set; } // viewmodel rotate left/right
        public static ConfigEntry<float> HandRecoilAngReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> HandRecoilAngDamping { get; set; } // viewmodel recoil damping

        // pistol ang
        public static ConfigEntry<float> PistolHandRecoilAngUpMult { get; set; } // pistol viewmodel rotate up/down
        public static ConfigEntry<float> PistolHandRecoilAngSideMult { get; set; } // pistol viewmodel rotate left/right
        public static ConfigEntry<float> PistolHandRecoilAngReturnSpeed { get; set; } // pistol viewmodel recoil speed
        public static ConfigEntry<float> PistolHandRecoilAngDamping { get; set; } // pistol viewmodel recoil damping

        private void Awake()
        {
            Util.Logger = Logger;

            CameraRecoilUpMult = Config.Bind(Category.CameraRecoil, "Camera Recoil Up Mult", -0.7f, new ConfigDescription("Multiplier for vertical camera recoil", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 1000 }));
            CameraRecoilSideMult = Config.Bind(Category.CameraRecoil, "Camera Recoil Side Mult", 1.0f, new ConfigDescription("Multiplier for sideways camera recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 990 }));
            CameraRecoilIntensity = Config.Bind(Category.CameraRecoil, "Camera Recoil Intensity", 2.0f, new ConfigDescription("Camera recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            CameraRecoilReturnSpeed = Config.Bind(Category.CameraRecoil, "Camera Recoil Return Speed", 0.6f, new ConfigDescription("Camera recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            CameraRecoilDamping = Config.Bind(Category.CameraRecoil, "Camera Recoil Damping", 0.3f, new ConfigDescription("Camera recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));

            HandRecoilPosBackMult = Config.Bind(Category.RecoilPos, "Recoil Position Backwards Mult", 2.0f, new ConfigDescription("Multiplier for backwards hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            HandRecoilPosIntensity = Config.Bind(Category.RecoilPos, "Recoil Position Intensity", 1.5f, new ConfigDescription("Hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            HandRecoilPosReturnSpeed = Config.Bind(Category.RecoilPos, "Recoil Position Return Speed", 0.7f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            HandRecoilPosDamping = Config.Bind(Category.RecoilPos, "Recoil Position Damping", 0.3f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));

            PistolHandRecoilPosBackMult = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Backwards Mult", 5f, new ConfigDescription("Multiplier for backwards pistol hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            PistolHandRecoilPosIntensity = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Intensity", 1f, new ConfigDescription("Pistol hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolHandRecoilPosReturnSpeed = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Return Speed", 0.5f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            PistolHandRecoilPosDamping = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Damping", 0.3f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));

            HandRecoilAngUpMult = Config.Bind(Category.RecoilAng, "Recoil Rotation Up Mult", 0.6f, new ConfigDescription("Multiplier for hand recoil angle up/down.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            HandRecoilAngSideMult = Config.Bind(Category.RecoilAng, "Recoil Rotation Side Mult", 1.3f, new ConfigDescription("Multiplier for sideways hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            HandRecoilAngReturnSpeed = Config.Bind(Category.RecoilAng, "Recoil Rotation Return Speed", 4.0f, new ConfigDescription("Hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));
            HandRecoilAngDamping = Config.Bind(Category.RecoilAng, "Recoil Rotation Damping", 0.6f, new ConfigDescription("Hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 920 }));

            PistolHandRecoilAngUpMult = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Up Mult", 20f, new ConfigDescription("Multiplier for pistol hand recoil angle up/down.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 970 }));
            PistolHandRecoilAngSideMult = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Side Mult", 2f, new ConfigDescription("Multiplier for pistol hand recoil left/right.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolHandRecoilAngReturnSpeed = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Return Speed", 35f, new ConfigDescription("Pistol hand recoil return speed.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolHandRecoilAngDamping = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Damping", 0.45f, new ConfigDescription("Pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));

            EnableCrankRecoil = Config.Bind(Category.General, "Enable Crank Recoil", true, new ConfigDescription("Toggles whether your weapon recoils toward the screen.", null, new ConfigurationManagerAttributes { Order = 950 }));
            CameraSnap = Config.Bind(Category.General, "Camera Snap Speed", 1f, new ConfigDescription("Speed at which the camera follows the weapon's recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolCameraSnap = Config.Bind(Category.General, "Pistol Camera Snap Speed", 0.5f, new ConfigDescription("Speed at which the camera follows the weapon's recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            AllowServerOverride = Config.Bind(Category.General, "Allow Server Override", true, new ConfigDescription("Allows the server to override client-side recoil settings. Currently required for some unique weapon recoils (Deagle, Glock 18c, etc.)", null, new ConfigurationManagerAttributes { Order = 920 }));
            AllowLeanCameraTilt = Config.Bind(Category.General, "Allow Lean Camera Tilt", false, new ConfigDescription("Changes whether the camera rotates during leaning.", null, new ConfigurationManagerAttributes { Order = 910 }));
            AllowStableRecoil = Config.Bind(Category.General, "Allow Vanilla Stable Recoil", true, new ConfigDescription("Allows vanilla recoil stabilization.", null, new ConfigurationManagerAttributes { Order = 909 }));

            LeftStanceOffset = Config.Bind(Category.LeftStance, "Left Stance Offset", 0.2f, new ConfigDescription("Offset for the left stance position.", new AcceptableValueRange<float>(0f, 0.2f), new ConfigurationManagerAttributes { Order = 900 }));
            LeftStanceAngle = Config.Bind(Category.LeftStance, "Left Stance Angle", 5f, new ConfigDescription("Angle for the left stance position.", new AcceptableValueRange<float>(-45f, 45f), new ConfigurationManagerAttributes { Order = 890 }));
            LeftStanceSpeed = Config.Bind(Category.LeftStance, "Left Stance Speed", 4f, new ConfigDescription("Speed at which the weapon moves when transitioning shoulders.", new AcceptableValueRange<float>(1f, 10f), new ConfigurationManagerAttributes { Order = 880 }));

            EnableRealRecoil = Config.Bind(Category.ReallyReal, "Enable Real Recoil", true, new ConfigDescription("Enables real recoil, which moves the camera while shooting. The camera will kick upward and left/right randomly. The amount depends on your weapon's stats and the multipliers below.", null, new ConfigurationManagerAttributes { Order = 870 }));
            RealRecoilVerticalMult = Config.Bind(Category.ReallyReal, "Real Recoil Vertical Mult", 0.7f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 860 }));
            RealRecoilHorizontalMult = Config.Bind(Category.ReallyReal, "Real Recoil Horizontal Mult", 0.5f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 850 }));
            RealRecoilPistolVerticalMult = Config.Bind(Category.ReallyReal, "Pistol Real Recoil Vertical Mult", 0.2f, new ConfigDescription("Real recoil vertical multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 840 }));
            RealRecoilPistolHorizontalMult = Config.Bind(Category.ReallyReal, "Pistol Real Recoil Horizontal Mult", 0.1f, new ConfigDescription("Real recoil horizontal multiplier.", new AcceptableValueRange<float>(0f, 10f), new ConfigurationManagerAttributes { Order = 830 }));
            RealRecoilDecaySpeed = Config.Bind(Category.ReallyReal, "Real Recoil Decay Speed", 20f, new ConfigDescription("Real recoil decay speed.", null, new ConfigurationManagerAttributes { Order = 820 }));
            RealRecoilMountedMult = Config.Bind(Category.ReallyReal, "Real Recoil Mounted Multiplier", 0.5f, new ConfigDescription("Changes the amount of recoil while mounted or using bipods.", null, new ConfigurationManagerAttributes { Order = 810 }));
            RealRecoilAimingMult = Config.Bind(Category.ReallyReal, "Real Recoil Aiming Multiplier", 0.75f, new ConfigDescription("Changes the amount of recoil while aiming.", null, new ConfigurationManagerAttributes { Order = 800 }));

            new RecoilProcessPatch().Enable();
            new UpdateWeaponVariablesPatch().Enable();
            new ToggleLeftStancePatch().Enable();
            new ApplyComplexRotationPatch().Enable();
            new WeaponOverlapLeftStancePatch().Enable();
            new CameraRecoilRotationPatch().Enable();
            new CamereLeanPatch().Enable();
            new PlayerInitPatch().Enable();
            new ShootPatch().Enable();
            new SetStableModePatch().Enable();

            List<WeaponRecoilData> recoilData = RouteHelper.FetchWeaponDataFromServer();
            WeaponRecoils = recoilData;
        }
    }
}
