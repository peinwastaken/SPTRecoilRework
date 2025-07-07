using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using EFT.InventoryLogic;
using HarmonyLib;
using PeinRecoilRework.Components;
using PeinRecoilRework.Data;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace PeinRecoilRework.Patches
{
    public class UpdateWeaponVariablesPatch : ModulePatch
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
            NewRecoilShotEffect recoilEffect = shotEffector.NewShotRecoil;
            RecoilProcessBase camAngRecoil = recoilEffect.CameraRotationRecoil;
            NewRotationRecoilProcess handAngRecoil = recoilEffect.HandRotationRecoil;
            RecoilProcessBase handPosRecoil = recoilEffect.HandPositionRecoil;
            Player.FirearmController firearmController = recoilEffect._firearmController;
            Weapon weapon = firearmController?.Item;
            WeaponTemplate template = weapon?.Template;
            string weaponId = weapon?.StringTemplateId ?? string.Empty;
            WeaponRecoilData customData = WeaponHelper.FindRecoilData(weaponId);
            bool isPistol = WeaponHelper.IsPistol(template);
            Player player = firearmController?.gameObject.GetComponent<Player>();

            if (player == null || player != Util.GetLocalPlayer())
            {
                return;
            }

            float handAngReturnSpeed = isPistol ? Plugin.PistolHandRecoilAngReturnSpeed.Value : Plugin.HandRecoilAngReturnSpeed.Value;
            float handAngDamping = isPistol ? Plugin.PistolHandRecoilAngDamping.Value : Plugin.HandRecoilAngDamping.Value;

            float handPosIntensity = isPistol ? Plugin.PistolHandRecoilPosIntensity.Value : Plugin.HandRecoilPosIntensity.Value;
            float handPosReturnSpeed = isPistol ? Plugin.PistolHandRecoilPosReturnSpeed.Value : Plugin.HandRecoilPosReturnSpeed.Value;
            float handPosDamping = isPistol ? Plugin.PistolHandRecoilPosDamping.Value : Plugin.HandRecoilPosDamping.Value;

            float cameraSnap = isPistol ? Plugin.PistolCameraSnap.Value : Plugin.CameraSnap.Value;

            __instance.CrankRecoil = Plugin.EnableCrankRecoil.Value;

            camAngRecoil.Intensity = Plugin.CameraRecoilIntensity.Value;
            camAngRecoil.ReturnSpeed = Plugin.CameraRecoilReturnSpeed.Value;
            camAngRecoil.Damping = Plugin.CameraRecoilDamping.Value;

            if (customData != null && Plugin.AllowServerOverride.Value == true)
            {
                handAngRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilAngReturnSpeed ?? handAngReturnSpeed;
                handAngRecoil.Damping = customData.OverrideProperties.HandRecoilAngDamping ?? handAngDamping;

                handPosRecoil.Intensity = customData.OverrideProperties.HandRecoilPosIntensity ?? handPosIntensity;
                handPosRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilPosReturnSpeed ?? handPosReturnSpeed;
                handPosRecoil.Damping = customData.OverrideProperties.HandRecoilPosDamping ?? handPosDamping;

                __instance.CameraSmoothRecoil = customData.OverrideProperties.CameraSnap ?? cameraSnap;
            }
            else
            {
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

    public class CameraRecoilRotationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.method_19));
        }

        [PatchPrefix]
        private static bool PatchPrefix(ProceduralWeaponAnimation __instance, float deltaTime)
        {
            return false;
        }
    }

    public class CamereLeanPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.CalculateCameraPosition));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            Vector3 zero = __instance.HandsContainer.CameraRotation.Zero;
            float lean = Plugin.AllowLeanCameraTilt.Value ? zero.z : 0f;
            __instance.HandsContainer.CameraRotation.Zero = new Vector3(zero.x, zero.y, lean);
        }
    }

    public class ApplyComplexRotationPatch : ModulePatch
    {
        private static float leftStanceTarget = 0f;
        private static float leftStanceMult = 0f;
        private static FieldInfo strategyField;

        protected override MethodBase GetTargetMethod()
        {
            strategyField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_strategy");
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.ApplyComplexRotation));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance, float dt)
        {
            Transform weaponRootAnim = __instance.HandsContainer.WeaponRootAnim;
            float offset = Plugin.LeftStanceOffset.Value;
            float angle = Plugin.LeftStanceAngle.Value;

            leftStanceTarget = WeaponHelper.IsLeftStance ? 1f : 0f;
            leftStanceMult = Mathf.Lerp(leftStanceMult, leftStanceTarget, dt * Plugin.LeftStanceSpeed.Value);
            WeaponHelper.LeftStanceMult = leftStanceMult;

            Vector3 leftStanceOffset = new Vector3(-offset * leftStanceMult, 0f, 0f);
            Quaternion rotationOffset = Quaternion.Euler(0f, -angle * leftStanceMult, 0f);

            weaponRootAnim.localPosition = weaponRootAnim.localPosition + leftStanceOffset;
            weaponRootAnim.localRotation = weaponRootAnim.localRotation * rotationOffset;
        }
    }

    public class ShootPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.Shoot));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            if (Plugin.EnableRealRecoil.Value == false) return;

            ShotEffector shotEffector = __instance.Shootingg;
            NewRecoilShotEffect newRecoil = shotEffector.NewShotRecoil;
            Player.FirearmController fc = shotEffector._firearmController;
            Player player = fc.gameObject.GetComponent<Player>();
            RealRecoilComponent realRecoil = player.gameObject.GetComponent<RealRecoilComponent>();

            bool isMounted = __instance.IsMountedState || __instance.IsBipodUsed || __instance.IsVerticalMounting;
            bool isPistol = WeaponHelper.IsPistol(fc.Weapon.Template);
            Vector2 rotationStrength = newRecoil.BasicPlayerRecoilRotationStrength;
            float recoilStrength = Random.Range(rotationStrength.x, rotationStrength.y);

            float verticalMult = isPistol ? Plugin.RealRecoilPistolVerticalMult.Value : Plugin.RealRecoilVerticalMult.Value;
            float horizontalMult = isPistol ? Plugin.RealRecoilPistolHorizontalMult.Value : Plugin.RealRecoilHorizontalMult.Value;
            float mountedMult = isMounted ? Plugin.RealRecoilMountedMult.Value : 1f;
            float aimingMult = __instance.IsAiming ? Plugin.RealRecoilAimingMult.Value : 1f;
            float recoilVertical = fc.Weapon.Template.RecoilForceUp * 0.003f * mountedMult * aimingMult * verticalMult;
            float recoilHorizontal = fc.Weapon.Template.RecoilForceBack * 0.001f * mountedMult * aimingMult * horizontalMult;

            realRecoil?.ApplyRecoil(recoilVertical, recoilHorizontal);
        }
    }
}
