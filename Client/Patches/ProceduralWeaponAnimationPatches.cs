using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using EFT.InventoryLogic;
using HarmonyLib;
using PeinRecoilRework.Components;
using PeinRecoilRework.Config.Settings;
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
            NewRotationRecoilProcess AngRecoil = recoilEffect.HandRotationRecoil;
            RecoilProcessBase PosRecoil = recoilEffect.HandPositionRecoil;
            Player.FirearmController firearmController = recoilEffect._firearmController;
            Weapon weapon = firearmController?.Item;
            WeaponTemplate template = weapon?.Template;
            string weaponId = weapon?.StringTemplateId ?? string.Empty;
            WeaponRecoilData customData = WeaponHelper.FindRecoilData(weaponId);
            bool isPistol = WeaponHelper.IsPistol(template);
            Player player = firearmController?.gameObject.GetComponent<Player>();
            CameraOffsetComponent cameraOffset = player?.GetComponent<CameraOffsetComponent>();

            if (player == null || player != Util.GetLocalPlayer())
            {
                return;
            }

            float AngReturnSpeed = isPistol ? PistolRecoilAngSettings.PistolRecoilAngReturnSpeed.Value : RecoilAngSettings.RecoilAngReturnSpeed.Value;
            float AngDamping = isPistol ? PistolRecoilAngSettings.PistolRecoilAngDamping.Value : RecoilAngSettings.RecoilAngDamping.Value;

            float PosIntensity = isPistol ? PistolRecoilPosSettings.PistolRecoilPosIntensity.Value : RecoilPosSettings.RecoilPosIntensity.Value;
            float PosReturnSpeed = isPistol ? PistolRecoilPosSettings.PistolRecoilPosReturnSpeed.Value : RecoilPosSettings.RecoilPosReturnSpeed.Value;
            float PosDamping = isPistol ? PistolRecoilPosSettings.PistolRecoilPosDamping.Value : RecoilPosSettings.RecoilPosDamping.Value;

            float cameraSnap = isPistol ? GeneralSettings.PistolCameraSnap.Value : GeneralSettings.CameraSnap.Value;

            __instance.CrankRecoil = GeneralSettings.EnableCrankRecoil.Value;

            camAngRecoil.Intensity = CameraRecoilSettings.CameraRecoilIntensity.Value;
            camAngRecoil.ReturnSpeed = CameraRecoilSettings.CameraRecoilReturnSpeed.Value;
            camAngRecoil.Damping = CameraRecoilSettings.CameraRecoilDamping.Value;

            if (customData != null && GeneralSettings.AllowServerOverride.Value == true)
            {
                AngRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilAngReturnSpeed ?? AngReturnSpeed;
                AngRecoil.Damping = customData.OverrideProperties.HandRecoilAngDamping ?? AngDamping;

                PosRecoil.Intensity = customData.OverrideProperties.HandRecoilPosIntensity ?? PosIntensity;
                PosRecoil.ReturnSpeed = customData.OverrideProperties.HandRecoilPosReturnSpeed ?? PosReturnSpeed;
                PosRecoil.Damping = customData.OverrideProperties.HandRecoilPosDamping ?? PosDamping;

                __instance.CameraSmoothRecoil = customData.OverrideProperties.CameraSnap ?? cameraSnap;
            }
            else
            {
                AngRecoil.ReturnSpeed = AngReturnSpeed;
                AngRecoil.Damping = AngDamping;

                PosRecoil.Intensity = PosIntensity;
                PosRecoil.ReturnSpeed = PosReturnSpeed;
                PosRecoil.Damping = PosDamping;

                __instance.CameraSmoothRecoil = cameraSnap;
            }

            if (cameraOffset != null)
            {
                RecoilSpring fastSpring = cameraOffset.FastShakeSpring;
                RecoilSpring slowSpring = cameraOffset.SlowShakeSpring;
                RecoilSpring cameraSpring = cameraOffset.CameraSpring;

                fastSpring.Damping = AdditionalCameraRecoilSettings.FastSpringDamping.Value;
                fastSpring.Speed = AdditionalCameraRecoilSettings.FastSpringSpeed.Value;
                fastSpring.Stiffness = AdditionalCameraRecoilSettings.FastSpringStiffness.Value;

                slowSpring.Damping = AdditionalCameraRecoilSettings.SlowSpringDamping.Value;
                slowSpring.Speed = AdditionalCameraRecoilSettings.SlowSpringSpeed.Value;
                slowSpring.Stiffness = AdditionalCameraRecoilSettings.SlowSpringStiffness.Value;

                cameraSpring.Damping = AdditionalCameraRecoilSettings.CameraSpringDamping.Value;
                cameraSpring.Speed = AdditionalCameraRecoilSettings.CameraSpringSpeed.Value;
                cameraSpring.Stiffness = AdditionalCameraRecoilSettings.CameraSpringStiffness.Value;
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

    public class CameraLeanPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.CalculateCameraPosition));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            Vector3 zero = __instance.HandsContainer.CameraRotation.Zero;
            float lean = GeneralSettings.AllowLeanCameraTilt.Value ? zero.z : 0f;
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
            float offset = LeftStanceSettings.LeftStanceOffset.Value;
            float angle = LeftStanceSettings.LeftStanceAngle.Value;

            leftStanceTarget = WeaponHelper.IsLeftStance ? 1f : 0f;
            leftStanceMult = Mathf.Lerp(leftStanceMult, leftStanceTarget, dt * LeftStanceSettings.LeftStanceSpeed.Value);
            WeaponHelper.LeftStanceMult = leftStanceMult;

            Vector3 leftStanceOffset = new Vector3(-offset * leftStanceMult, 0f, 0f);
            Quaternion rotationOffset = Quaternion.Euler(0f, -angle * leftStanceMult, 0f);

            weaponRootAnim.localPosition = weaponRootAnim.localPosition + leftStanceOffset;
            weaponRootAnim.localRotation = weaponRootAnim.localRotation * rotationOffset;
        }
    }

    public class LerpCameraPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.LerpCamera));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance, float dt)
        {
            Vector2 scale = new Vector2(0.01f, 0.01f);

            Player player = Util.GetLocalPlayer();
            CameraOffsetComponent cameraShake = player.gameObject.GetComponent<CameraOffsetComponent>();
            if (cameraShake == null)
            {
                return;
            }

            Vector3 slowShake = cameraShake.SlowShakeSpring.Position;
            Vector3 fastShake = cameraShake.FastShakeSpring.Position;

            Vector3 cameraRecoil = cameraShake.CameraSpring.Position;

            Vector3 cameraPosOffset = slowShake + fastShake;
            Vector3 cameraAngOffset = cameraRecoil;

            cameraPosOffset *= scale;

            __instance.HandsContainer.CameraTransform.localPosition += cameraPosOffset;
            __instance.HandsContainer.CameraTransform.localEulerAngles += cameraAngOffset;
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
            float scaleVert = 0.01f;
            float scaleHor = 0.01f;

            ShotEffector shotEffector = __instance.Shootingg;
            Player.FirearmController fc = shotEffector._firearmController;
            Player player = fc.gameObject.GetComponent<Player>();
            RealRecoilComponent realRecoil = player.gameObject.GetComponent<RealRecoilComponent>();
            CameraOffsetComponent cameraShake = player.gameObject.GetComponent<CameraOffsetComponent>();

            if (RealRecoilSettings.EnableRealRecoil.Value == true)
            {
                bool isMounted = __instance.IsMountedState || __instance.IsBipodUsed || __instance.IsVerticalMounting;
                bool isPistol = WeaponHelper.IsPistol(fc.Weapon.Template);
                float recoilStr = shotEffector.NewShotRecoil.BasicPlayerRecoilRotationStrength.y;

                float verticalMult = isPistol ? RealRecoilSettings.RealRecoilPistolVerticalMult.Value : RealRecoilSettings.RealRecoilVerticalMult.Value;
                float horizontalMult = isPistol ? RealRecoilSettings.RealRecoilPistolHorizontalMult.Value : RealRecoilSettings.RealRecoilHorizontalMult.Value;
                float mountedMult = isMounted ? RealRecoilSettings.RealRecoilMountedMult.Value : 1f;
                float aimingMult = __instance.IsAiming ? RealRecoilSettings.RealRecoilAimingMult.Value : 1f;

                float recoilVertical = recoilStr * scaleVert * mountedMult * aimingMult * verticalMult;
                float recoilHorizontal = recoilStr * scaleHor * mountedMult * aimingMult * horizontalMult;

                realRecoil.ApplyRecoil(recoilVertical, recoilHorizontal);
            }

            if (AdditionalCameraRecoilSettings.EnableAdditionalCameraRecoil.Value == true)
            {
                cameraShake.DoRecoilShake();
            }
        }
    }
}
