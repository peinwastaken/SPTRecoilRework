using BepInEx;
using PeinRecoilRework.Config;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using PeinRecoilRework.Patches;
using System.Collections.Generic;

namespace PeinRecoilRework
{
    [BepInPlugin("com.pein.camerarecoilmod", "PeinRecoilRework", "1.5.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static List<WeaponRecoilData> WeaponRecoils { get; set; }        

        private void Awake()
        {
            Util.Logger = Logger;

            GeneralSettings.Bind(Config, 1, Category.General);
            LeftStanceSettings.Bind(Config, 2, Category.LeftStance);
            RealRecoilSettings.Bind(Config, 3, Category.ReallyReal);
            CameraRecoilSettings.Bind(Config, 4, Category.CameraRecoil);
            AdditionalCameraRecoilSettings.Bind(Config, 5, Category.AdditionalCamera);
            RecoilPosSettings.Bind(Config, 6, Category.RecoilPos);
            PistolRecoilPosSettings.Bind(Config, 7, Category.PistolRecoilPos);
            RecoilAngSettings.Bind(Config, 8, Category.RecoilAng);
            PistolRecoilAngSettings.Bind(Config, 9, Category.PistolRecoilAng);

            new RecoilProcessPatch().Enable();
            new UpdateWeaponVariablesPatch().Enable();
            new ToggleLeftStancePatch().Enable();
            new ApplyComplexRotationPatch().Enable();
            new WeaponOverlapLeftStancePatch().Enable();
            new CameraRecoilRotationPatch().Enable();
            new CameraLeanPatch().Enable();
            new PlayerInitPatch().Enable();
            new ShootPatch().Enable();
            new SetStableModePatch().Enable();
            new LerpCameraPatch().Enable();

            List<WeaponRecoilData> recoilData = RouteHelper.FetchWeaponDataFromServer();
            WeaponRecoils = recoilData;
        }
    }
}
