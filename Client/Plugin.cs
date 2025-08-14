using BepInEx;
using PeinRecoilRework.Config;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using PeinRecoilRework.Patches;

namespace PeinRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "PeinRecoilRework", "1.8.1")]
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
        }
    }
}