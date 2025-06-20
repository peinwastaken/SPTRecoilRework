using BepInEx;
using BepInEx.Configuration;
using PeinRecoilRework.Config;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using PeinRecoilRework.Patches;
using System.Collections.Generic;
using UnityEngine;

namespace PeinRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "PeinRecoilRework", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static List<WeaponRecoilData> WeaponRecoils { get; set; }

        // general settings
        public static ConfigEntry<bool> EnableCrankRecoil { get; set; } // recoil backwards instead of forwards
        public static ConfigEntry<float> CameraSnap { get; set; } // camera snap speed
        public static ConfigEntry<float> PistolCameraSnap { get; set; } // pistol camera snap speed
        public static ConfigEntry<Vector2> CameraToWeaponAngleSpeed { get; set; } // camera angle speed min, max
        public static ConfigEntry<bool> AllowServerOverride { get; set; } // allow server to override recoil settings

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
        public static ConfigEntry<float> HandRecoilAngIntensity { get; set; } // viewmodel recoil intensity
        public static ConfigEntry<float> HandRecoilAngReturnSpeed { get; set; } // viewmodel recoil speed
        public static ConfigEntry<float> HandRecoilAngDamping { get; set; } // viewmodel recoil damping

        // pistol ang
        public static ConfigEntry<float> PistolHandRecoilAngUpMult { get; set; } // pistol viewmodel rotate up/down
        public static ConfigEntry<float> PistolHandRecoilAngSideMult { get; set; } // pistol viewmodel rotate left/right
        public static ConfigEntry<float> PistolHandRecoilAngIntensity { get; set; } // pistol viewmodel recoil intensity
        public static ConfigEntry<float> PistolHandRecoilAngReturnSpeed { get; set; } // pistol viewmodel recoil speed
        public static ConfigEntry<float> PistolHandRecoilAngDamping { get; set; } // pistol viewmodel recoil damping

        private void Awake()
        {
            Util.Logger = Logger;

            CameraRecoilUpMult = Config.Bind(Category.CameraRecoil, "Camera Recoil Up Mult", -0.7f, new ConfigDescription("Multiplier for vertical camera recoil", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 1000 }));
            CameraRecoilSideMult = Config.Bind(Category.CameraRecoil, "Camera Recoil Side Mult", 1.0f, new ConfigDescription("Multiplier for sideways camera recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 990 }));
            CameraRecoilIntensity = Config.Bind(Category.CameraRecoil, "Camera Recoil Intensity", 2.0f, new ConfigDescription("Multiplier for camera recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            CameraRecoilReturnSpeed = Config.Bind(Category.CameraRecoil, "Camera Recoil Return Speed", 0.6f, new ConfigDescription("Multiplier for camerar recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            CameraRecoilDamping = Config.Bind(Category.CameraRecoil, "Camera Recoil Damping", 0.3f, new ConfigDescription("Multiplier for camera recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));

            HandRecoilPosBackMult = Config.Bind(Category.RecoilPos, "Recoil Position Backwards Mult", 2.0f, new ConfigDescription("Multiplier for backwards hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            HandRecoilPosIntensity = Config.Bind(Category.RecoilPos, "Recoil Position Intensity", 1.5f, new ConfigDescription("Multiplier for hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            HandRecoilPosReturnSpeed = Config.Bind(Category.RecoilPos, "Recoil Position Return Speed", 0.7f, new ConfigDescription("Multiplier for hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            HandRecoilPosDamping = Config.Bind(Category.RecoilPos, "Recoil Position Damping", 0.3f, new ConfigDescription("Multiplier for hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            
            PistolHandRecoilPosBackMult = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Backwards Mult", 5f, new ConfigDescription("Multiplier for backwards pistol hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 980 }));
            PistolHandRecoilPosIntensity = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Intensity", 1f, new ConfigDescription("Multiplier for pistol hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolHandRecoilPosReturnSpeed = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Return Speed", 0.5f, new ConfigDescription("Multiplier for pistol hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            PistolHandRecoilPosDamping = Config.Bind(Category.PistolRecoilPos, "Pistol Recoil Position Damping", 0.3f, new ConfigDescription("Multiplier for pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));

            HandRecoilAngUpMult = Config.Bind(Category.RecoilAng, "Recoil Rotation Up Mult", 0.6f, new ConfigDescription("Multiplier for hand recoil angle up/down.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 970 }));
            HandRecoilAngSideMult = Config.Bind(Category.RecoilAng, "Recoil Rotation Side Mult", 1.3f, new ConfigDescription("Multiplier for sideways hand recoil.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            HandRecoilAngIntensity = Config.Bind(Category.RecoilAng, "Recoil Rotation Intensity", 1.0f, new ConfigDescription("Multiplier for hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 940 }));
            HandRecoilAngReturnSpeed = Config.Bind(Category.RecoilAng, "Recoil Rotation Return Speed", 2.0f, new ConfigDescription("Multiplier for hand recoil return speed.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));
            HandRecoilAngDamping = Config.Bind(Category.RecoilAng, "Recoil Rotation Damping", 0.8f, new ConfigDescription("Multiplier for hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 920 }));

            PistolHandRecoilAngUpMult = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Up Mult", 10f, new ConfigDescription("Multiplier for pistol hand recoil angle up/down.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 970 }));
            PistolHandRecoilAngSideMult = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Side Mult", 4f, new ConfigDescription("Multiplier for pistol hand recoil left/right.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 960 }));
            PistolHandRecoilAngIntensity = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Intensity", 5f, new ConfigDescription("Multiplier for pistol hand recoil intensity.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 950 }));
            PistolHandRecoilAngReturnSpeed = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Return Speed", 25f, new ConfigDescription("Multiplier for pistol hand recoil return speed.", new AcceptableValueRange<float>(-50f, 50f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolHandRecoilAngDamping = Config.Bind(Category.PistolRecoilAng, "Pistol Recoil Rotation Damping", 0.4f, new ConfigDescription("Multiplier for pistol hand recoil damping.", new AcceptableValueRange<float>(-5f, 5f), new ConfigurationManagerAttributes { Order = 930 }));

            EnableCrankRecoil = Config.Bind(Category.General, "Enable Crank Recoil", true, new ConfigDescription("Toggles whether your weapon recoils toward the screen.", null, new ConfigurationManagerAttributes { Order = 950 }));
            CameraSnap = Config.Bind(Category.General, "Camera Snap Speed", 1f, new ConfigDescription("Speed at which the camera snaps back to its original position after recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            PistolCameraSnap = Config.Bind(Category.General, "Pistol Camera Snap Speed", 0.1f, new ConfigDescription("Speed at which the camera snaps back to its original position after pistol recoil.", new AcceptableValueRange<float>(0f, 2f), new ConfigurationManagerAttributes { Order = 940 }));
            CameraToWeaponAngleSpeed = Config.Bind(Category.General, "Camera to Weapon Angle Speed", new Vector2(0.0f, 0.0f), new ConfigDescription("Minimum and maximum speed at which the camera aligns with the weapon's angle.", null, new ConfigurationManagerAttributes { Order = 930 }));
            AllowServerOverride = Config.Bind(Category.General, "Allow Server Override", true, new ConfigDescription("Allows the server to override client-side recoil settings. Currently required for some unique weapon recoils (Deagle, Glock 18c, etc.)", null, new ConfigurationManagerAttributes { Order = 920 }));

            new RecoilProcessPatch().Enable();
            new ProceduralWeaponAnimationPatches().Enable();
            new RecoilStableModePatch().Enable();
            new ToggleLeftStancePatch().Enable();
            new ApplyComplexRotationPatch().Enable();

            List<WeaponRecoilData> recoilData = RouteHelper.FetchWeaponDataFromServer();
            WeaponRecoils = recoilData;
        }
    }
}
