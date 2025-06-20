using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using EFT.InventoryLogic;
using HarmonyLib;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace PeinRecoilRework.Patches
{
    public class ProceduralWeaponAnimationPatches : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.UpdateWeaponVariables));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            // variable hell
            ShotEffector shotEffector = __instance.Shootingg;
            NewRecoilShotEffect newRecoil = shotEffector.NewShotRecoil;
            RecoilProcessBase camAngRecoil = newRecoil.CameraRotationRecoil;
            NewRotationRecoilProcess handAngRecoil = newRecoil.HandRotationRecoil;
            RecoilProcessBase handPosRecoil = newRecoil.HandPositionRecoil;
            Player.FirearmController firearmController = newRecoil._firearmController;
            Weapon weapon = firearmController?.Item;
            WeaponTemplate template = weapon?.Template;
            string weaponId = weapon?.StringTemplateId ?? string.Empty;
            WeaponRecoilData customData = WeaponHelper.FindRecoilData(weaponId);
            bool isPistol = WeaponHelper.IsPistol(template);

            float handAngIntensity = isPistol ? Plugin.PistolHandRecoilAngIntensity.Value : Plugin.HandRecoilAngIntensity.Value;
            float handAngReturnSpeed = isPistol ? Plugin.PistolHandRecoilAngReturnSpeed.Value : Plugin.HandRecoilAngReturnSpeed.Value;
            float handAngDamping = isPistol ? Plugin.PistolHandRecoilAngDamping.Value : Plugin.HandRecoilAngDamping.Value;

            float handPosIntensity = isPistol ? Plugin.PistolHandRecoilPosIntensity.Value : Plugin.HandRecoilPosIntensity.Value;
            float handPosReturnSpeed = isPistol ? Plugin.PistolHandRecoilPosReturnSpeed.Value : Plugin.HandRecoilPosReturnSpeed.Value;
            float handPosDamping = isPistol ? Plugin.PistolHandRecoilPosDamping.Value : Plugin.HandRecoilPosDamping.Value;

            float cameraSnap = isPistol ? Plugin.PistolCameraSnap.Value : Plugin.CameraSnap.Value;

            __instance.CrankRecoil = Plugin.EnableCrankRecoil.Value;
            __instance.CameraToWeaponAngleSpeedRange = Plugin.CameraToWeaponAngleSpeed.Value;

            camAngRecoil.Intensity = Plugin.CameraRecoilIntensity.Value;
            camAngRecoil.ReturnSpeed = Plugin.CameraRecoilReturnSpeed.Value;
            camAngRecoil.Damping = Plugin.CameraRecoilDamping.Value;

            if (customData != null && Plugin.AllowServerOverride.Value == true)
            {
                handAngRecoil.Intensity = customData.OverrideProperties.HandRecoilAngIntensity ?? handAngIntensity;
                handAngRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilAngReturnSpeed ?? handAngReturnSpeed;
                handAngRecoil.Damping = customData.OverrideProperties.HandRecoilAngDamping ?? handAngDamping;

                handPosRecoil.Intensity = customData.OverrideProperties.HandRecoilPosIntensity ?? handPosIntensity;
                handPosRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilPosReturnSpeed ?? handPosReturnSpeed;
                handPosRecoil.Damping = customData.OverrideProperties.HandRecoilPosDamping ?? handPosDamping;

                __instance.CameraSmoothRecoil = customData.OverrideProperties.CameraSnap ?? cameraSnap;
            }
            else
            {
                handAngRecoil.Intensity = handAngIntensity;
                handAngRecoil.ReturnSpeed = handAngReturnSpeed;
                handAngRecoil.Damping = handAngDamping;

                handPosRecoil.Intensity = handPosIntensity;
                handPosRecoil.ReturnSpeed = handPosReturnSpeed;
                handPosRecoil.Damping = handPosDamping;

                __instance.CameraSmoothRecoil = cameraSnap;
            }

            WeaponHelper.IsPistolCurrentlyEquipped = isPistol;
            WeaponHelper.CurrentTemplate = template;
        }
    }

    public class ApplyComplexRotationPatch : ModulePatch
    {
        private static float leftStanceTarget = 0f;
        private static float leftStanceMult = 0f;
        private static FieldInfo strategyField;
        private static Vector3 originalLocalPos;
        private static bool posCached = false;

        protected override MethodBase GetTargetMethod()
        {
            strategyField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_strategy");
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.ApplyComplexRotation));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance, float dt)
        {
            Transform weaponRoot = __instance.HandsContainer.WeaponRootAnim;
            GInterface38 strategy = (GInterface38)strategyField.GetValue(__instance);

            leftStanceTarget = WeaponHelper.IsLeftStance ? 1f : 0f;
            leftStanceMult = Mathf.Lerp(leftStanceMult, leftStanceTarget, Time.deltaTime * 4f);

            Util.Logger.LogInfo($"leftStanceTarget: {leftStanceTarget}");
            Util.Logger.LogInfo($"leftStanceMult: {leftStanceMult}");

            Vector3 leftStanceOffset = new Vector3(-0.2f * leftStanceMult, 0f, 0f);
            Quaternion rotationOffset = Quaternion.Euler(0f, -5f * leftStanceMult, 0f);

            weaponRoot.SetPositionAndRotation(
                weaponRoot.position,
                weaponRoot.rotation
            );

            weaponRoot.localPosition = weaponRoot.localPosition + leftStanceOffset;
        }
    }
}
