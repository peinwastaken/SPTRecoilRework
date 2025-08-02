using HarmonyLib;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace PeinRecoilRework.Patches
{
    public class RecoilProcessPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ShotEffector.RecoilShotVal), nameof(ShotEffector.RecoilShotVal.Process));
        }

        [PatchPrefix]
        private static bool PatchPrefix(ShotEffector.RecoilShotVal __instance, ref Vector3 rnd)
        {
            Target processType = __instance.ProcessType;
            WeaponRecoilData customData = WeaponHelper.FindRecoilData(WeaponHelper.CurrentTemplate?.StringId ?? string.Empty);
            bool pistolEquipped = WeaponHelper.IsPistolCurrentlyEquipped;

            float posBackMult = pistolEquipped ? PistolRecoilPosSettings.PistolRecoilPosBackMult.Value : RecoilPosSettings.RecoilPosBackMult.Value;
            float angUpMult = pistolEquipped ? PistolRecoilAngSettings.PistolRecoilAngUpMult.Value : RecoilAngSettings.RecoilAngUpMult.Value;
            float angSideMult = pistolEquipped ? PistolRecoilAngSettings.PistolRecoilAngSideMult.Value : RecoilAngSettings.RecoilAngSideMult.Value;

            if (processType == Target.CameraRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= CameraRecoilSettings.CameraRecoilUpMult.Value;
                newVector.y *= CameraRecoilSettings.CameraRecoilSideMult.Value;
                rnd = newVector;
            }

            if (customData != null && GeneralSettings.AllowServerOverride.Value == true)
            {
                if (processType == Target.HandsPosition)
                {
                    Vector3 newVector = rnd;
                    newVector.z *= customData.OverrideProperties.HandRecoilPosBackMult ?? posBackMult;
                    rnd = newVector;
                }

                if (processType == Target.HandsRotation)
                {
                    Vector3 newVector = rnd;
                    newVector.x *= customData.OverrideProperties.HandRecoilAngUpMult ?? angUpMult;
                    newVector.y *= customData.OverrideProperties.HandRecoilAngSideMult ?? angSideMult;
                    rnd = newVector;
                }
            }
            else
            {
                if (processType == Target.HandsPosition)
                {
                    DebugLogger.LogInfo($"handspos back force: {rnd.z}");

                    Vector3 newVector = rnd;
                    newVector.z *= posBackMult;
                    rnd = newVector;
                }

                if (processType == Target.HandsRotation)
                {
                    Vector3 newVector = rnd;
                    newVector.x *= angUpMult;
                    newVector.y *= angSideMult;
                    rnd = newVector;
                }
            }

            return true;
        }
    }
}
