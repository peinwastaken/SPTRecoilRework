using BepInEx;
using SPTRecoilRework.Config;
using SPTRecoilRework.Config.Settings;
using SPTRecoilRework.Helpers;
using SPTRecoilRework.Patches;

namespace SPTRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "SPTRecoilRework", "1.10.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            DebugLogger.Logger = Logger;

            DebugSettings.Bind(Config, 0, Category.Debug);
            GeneralSettings.Bind(Config, 1, Category.General);
            // WeaponSwaySettings.Bind(Config, 3, Category.WeaponSway);
            RealRecoilSettings.Bind(Config, 2, Category.ReallyReal);
            CameraRecoilSettings.Bind(Config, 3, Category.CameraRecoil);
            AdditionalCameraRecoilSettings.Bind(Config, 4, Category.AdditionalCamera);
            RecoilSettings.Bind(Config, 5, Category.RecoilSettings);
            PistolRecoilSettings.Bind(Config, 6, Category.PistolRecoilSettings);

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
            new UpdateHipAccuracyPatch().Enable();
        }
    }
}