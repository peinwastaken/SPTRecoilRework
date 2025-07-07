using HarmonyLib;
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
            bool isLeftStance = WeaponHelper.IsLeftStance; // temporary

            float posBackMult = pistolEquipped ? Plugin.PistolHandRecoilPosBackMult.Value : Plugin.HandRecoilPosBackMult.Value;
            float angUpMult = pistolEquipped ? Plugin.PistolHandRecoilAngUpMult.Value : Plugin.HandRecoilAngUpMult.Value;
            float angSideMult = pistolEquipped ? Plugin.PistolHandRecoilAngSideMult.Value : Plugin.HandRecoilAngSideMult.Value;

            if (processType == Target.CameraRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= Plugin.CameraRecoilUpMult.Value;
                newVector.y *= Plugin.CameraRecoilSideMult.Value;
                rnd = newVector;
            }

            if (customData != null && Plugin.AllowServerOverride.Value == true)
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
