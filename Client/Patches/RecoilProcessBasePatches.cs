using EFT.Animations;
using EFT.Animations.NewRecoil;
using HarmonyLib;
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

            if (processType == Target.CameraRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= Plugin.CameraRecoilUpMult.Value;
                newVector.y *= Plugin.CameraRecoilSideMult.Value;
                rnd = newVector;
            }

            if (processType == Target.HandsPosition)
            {
                Vector3 newVector = rnd;
                newVector.z *= Plugin.HandRecoilPosBackMult.Value;
                rnd = newVector;
            }

            if (processType == Target.HandsRotation)
            {
                Vector3 newVector = rnd;
                newVector.x *= Plugin.HandRecoilAngUpMult.Value;
                newVector.z *= Plugin.HandRecoilAngSideMult.Value;
                rnd = newVector;
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
