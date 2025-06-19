using EFT.Animations;
using EFT.Animations.NewRecoil;
using HarmonyLib;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System;
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

            Util.Logger.LogInfo(rnd.ToString());

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
                    newVector.z *= customData.OverrideProperties.HandRecoilAngSideMult ?? angSideMult;
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
                    newVector.z *= angSideMult;
                    rnd = newVector;
                }
            }

            return true;
        }
    }

    public class RecoilCalculatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRecoilShotEffect), nameof(NewRecoilShotEffect.method_3));
        }

        [PatchPrefix]
        private static bool PatchPrefix(NewRecoilShotEffect __instance, out Vector2 recoilRadian)
        {
            Vector2 vector = default;

            foreach (ShotsGroupSettings shotsGroupSettings in __instance.ShotsGroupsSettings)
            {
                if (shotsGroupSettings.IsShotIndexInRange(__instance._autoFireShotIndex))
                {
                    vector += shotsGroupSettings.ShotRecoilRadianRange;
                }
            }
            if (__instance.HandRotationRecoil.StableOn)
            {
                __instance.HandRotationRecoil.CurrentAngleAdd += __instance.HandRotationRecoil.StableAngleIncreaseStep;
                __instance.HandRotationRecoil.CurrentAngleAdd = Mathf.Clamp(__instance.HandRotationRecoil.CurrentAngleAdd, __instance.HandRotationRecoil.ProgressRecoilAngleOnStable.x, __instance.HandRotationRecoil.ProgressRecoilAngleOnStable.y);
                vector += new Vector2(-__instance.HandRotationRecoil.CurrentAngleAdd, __instance.HandRotationRecoil.CurrentAngleAdd) * 0.2f;
            }

            recoilRadian = __instance.BasicRecoilRadian + vector * 0.017453292f;

            return false;
        }
    }

    public class RecoilStableModePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRotationRecoilProcess), nameof(NewRotationRecoilProcess.SetStableMode));
        }

        [PatchPrefix]
        private static bool PatchPrefix(ref bool enable)

        {
            //enable = false;
            return true;
        }
    }
}
