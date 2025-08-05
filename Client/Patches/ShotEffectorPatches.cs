using HarmonyLib;
using PeinRecoilRework.Config.Settings;
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
            bool pistolEquipped = WeaponHelper.IsPistolCurrentlyEquipped;

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
                Vector2 recoilVals = WeaponHelper.CurrentRecoilVals;
                float multModifier = WeaponHelper.GetDynamicRecoilRange(recoilVals.x, RecoilSettings.DynamicReturnMinMax.Value);

                Vector3 newVector = rnd;
                newVector.z *= posBackMult * multModifier;
                rnd = newVector;
            }

            if (processType == Target.HandsRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= angUpMult;
                newVector.y *= angSideMult;
                rnd = newVector;
            }

            return true;
        }
    }
}
