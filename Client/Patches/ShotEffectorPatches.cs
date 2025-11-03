using HarmonyLib;
using SPT.Reflection.Patching;
using SPTRecoilRework.Config.Settings;
using SPTRecoilRework.Helpers;
using System.Reflection;
using UnityEngine;

namespace SPTRecoilRework.Patches
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
            bool pistolEquipped = WeaponHelper.IsPistolCurrentlyEquipped;
            Vector2 recoilVals = WeaponHelper.CurrentRecoilVals;
            Vector2 recoilRange = pistolEquipped ? PistolRecoilSettings.DynamicRangeMinMax.Value : RecoilSettings.DynamicRangeMinMax.Value;
            float posBackMult = pistolEquipped ? PistolRecoilSettings.RecoilPosBackMult.Value : RecoilSettings.RecoilPosBackMult.Value;
            float angUpMult = pistolEquipped ? PistolRecoilSettings.RecoilAngUpMult.Value : RecoilSettings.RecoilAngUpMult.Value;
            float angSideMult = pistolEquipped ? PistolRecoilSettings.RecoilAngSideMult.Value : RecoilSettings.RecoilAngSideMult.Value;

            if (processType == Target.CameraRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= CameraRecoilSettings.CameraRecoilUpMult.Value;
                newVector.y *= CameraRecoilSettings.CameraRecoilSideMult.Value;
                rnd = newVector;
            }

            if (processType == Target.HandsPosition)
            {
                float multModifier = pistolEquipped ? 1f : WeaponHelper.GetDynamicRecoilMult(recoilVals.x, recoilRange, RecoilSettings.DynamicMultMinMax.Value, pistolEquipped);

                Vector3 newVector = rnd;
                newVector.z *= posBackMult * multModifier;
                rnd = newVector;
            }

            if (processType == Target.HandsRotation)
            {
                float multModifier = pistolEquipped ? WeaponHelper.GetDynamicRecoilMult(recoilVals.y, recoilRange, PistolRecoilSettings.DynamicMultMinMax.Value, pistolEquipped) : 1f ;

                Vector3 newVector = rnd;
                newVector.x *= angUpMult * multModifier;
                newVector.y *= angSideMult;
                rnd = newVector;
            }

            return true;
        }
    }
}
