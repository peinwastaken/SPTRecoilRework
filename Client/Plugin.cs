using BepInEx;
using PeinRecoilRework.Config;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using PeinRecoilRework.Patches;
using System.Collections.Generic;

namespace PeinRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "PeinRecoilRework", "1.8.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static List<WeaponRecoilData> WeaponRecoils { get; set; }        

        private void Awake()
        {
            DebugLogger.Logger = Logger;

            DebugSettings.Bind(Config, 0, Category.Debug);
            GeneralSettings.Bind(Config, 1, Category.General);
            // WeaponSwaySettings.Bind(Config, 3, Category.WeaponSway);
            RealRecoilSettings.Bind(Config, 2, Category.ReallyReal);
            CameraRecoilSettings.Bind(Config, 3, Category.CameraRecoil);
            AdditionalCameraRecoilSettings.Bind(Config, 4, Category.AdditionalCamera);
            RecoilPosSettings.Bind(Config, 5, Category.RecoilPos);
            PistolRecoilPosSettings.Bind(Config, 6, Category.PistolRecoilPos);
            RecoilAngSettings.Bind(Config, 7, Category.RecoilAng);
            PistolRecoilAngSettings.Bind(Config, 8, Category.PistolRecoilAng);
            WeaponRecoilSettings.Bind(Config, 9, Category.WeaponRecoilSettings);
            PistolWeaponRecoilSettings.Bind(Config, 10, Category.PistolWeaponRecoilSettings);

            new RecoilProcessPatch().Enable();
            new CameraRecoilRotationPatch().Enable();
            new CameraLeanPatch().Enable();
            new PlayerInitPatch().Enable();
            new ShootPatch().Enable();
            new SetStableModePatch().Enable();
            new LerpCameraPatch().Enable();
            new RecalculateRecoilOnWeaponSwitchPatch().Enable();
            new SetPlayerAimingPatch().Enable();
            new WeaponRecoilInitializePatch().Enable();
            new ShiftWeaponRootPatch().Enable();

            List<WeaponRecoilData> recoilData = RouteHelper.FetchWeaponDataFromServer();
            WeaponRecoils = recoilData;
        }
    }
}
