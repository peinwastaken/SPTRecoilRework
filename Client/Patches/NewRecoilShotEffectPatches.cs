using BepInEx.Configuration;
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
    public class RecalculateRecoilOnWeaponSwitchPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRecoilShotEffect), nameof(NewRecoilShotEffect.RecalculateRecoilParamsOnChangeWeapon));
        }

        [PatchPostfix]
        public static void PatchPostfix(
            NewRecoilShotEffect __instance,
            WeaponTemplate template,
            BackendConfigSettingsClass.AimingConfiguration AimingConfig,
            Player.FirearmController firearmController,
            float recoilSuppressionX,
            float recoilSuppressionY,
            float recoilSuppressionFactor,
            float modsFactorRecoil
        )
        {
            if (firearmController == null)
            {
                DebugLogger.LogWarning("firearmController was not found! please dont run the postfix! thanks!");
                return;
            }

            Player player = firearmController?.gameObject.GetComponent<Player>();
            if (player == null) return;
            ProceduralWeaponAnimation pwa = player.ProceduralWeaponAnimation;
            ShotEffector shotEffector = pwa.Shootingg;

            NewRecoilShotEffect recoilEffect = shotEffector.NewShotRecoil;
            RecoilProcessBase camAngRecoil = recoilEffect.CameraRotationRecoil;
            NewRotationRecoilProcess AngRecoil = recoilEffect.HandRotationRecoil;
            RecoilProcessBase PosRecoil = recoilEffect.HandPositionRecoil;

            EWeaponClass weaponClass = WeaponHelper.GetWeaponClass(template);
            string weaponId = template?.StringId ?? string.Empty;
            bool isPistol = WeaponHelper.IsPistol(template);
            Vector2 recoilVals = WeaponHelper.GetWeaponRecoilValues(WeaponHelper.CurrentFirearmController);
            float returnModifier = WeaponHelper.GetDynamicRecoilRange(recoilVals.x, RecoilSettings.DynamicReturnMinMax.Value);
            float dampingModifier = WeaponHelper.GetDynamicRecoilRange(recoilVals.x, RecoilSettings.DynamicDampingMinMax.Value);

            CameraOffsetComponent cameraOffset = player?.GetComponent<CameraOffsetComponent>();

            if (player == null || player != Util.GetLocalPlayer())
            {
                return;
            }

            // wtf is this!!!
            float angReturnSpeed = isPistol ? PistolRecoilSettings.RecoilAngReturnSpeed.Value : RecoilSettings.RecoilAngReturnSpeed.Value;
            float angDamping = isPistol ? PistolRecoilSettings.RecoilAngDamping.Value : RecoilSettings.RecoilAngDamping.Value;
            float posIntensity = isPistol ? PistolRecoilSettings.RecoilPosIntensity.Value : RecoilSettings.RecoilPosIntensity.Value;
            float posReturnSpeed = isPistol ? PistolRecoilSettings.RecoilPosReturnSpeed.Value : RecoilSettings.RecoilPosReturnSpeed.Value;
            float posDamping = isPistol ? PistolRecoilSettings.RecoilPosDamping.Value : RecoilSettings.RecoilPosDamping.Value;

            float cameraSnap = isPistol ? GeneralSettings.PistolCameraSnap.Value : GeneralSettings.CameraSnap.Value;
            bool useCategoryMult = RealRecoilSettings.EnableRealRecoilPerWeaponMults.Value;

            pwa.CrankRecoil = GeneralSettings.EnableCrankRecoil.Value;

            camAngRecoil.Intensity = CameraRecoilSettings.CameraRecoilIntensity.Value;
            camAngRecoil.ReturnSpeed = CameraRecoilSettings.CameraRecoilReturnSpeed.Value;
            camAngRecoil.Damping = CameraRecoilSettings.CameraRecoilDamping.Value;

            AngRecoil.ReturnSpeed = angReturnSpeed;
            AngRecoil.Damping = angDamping;

            PosRecoil.Intensity = posIntensity;
            PosRecoil.ReturnSpeed = posReturnSpeed * returnModifier;
            PosRecoil.Damping = posDamping * dampingModifier;

            pwa.CameraSmoothRecoil = cameraSnap;

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

            if (useCategoryMult)
            {
                ConfigEntry<Vector2> recoilConfig;
                WeaponHelper.RealRecoilMultipliers.TryGetValue(weaponClass, out recoilConfig);
                WeaponHelper.CurrentRecoilMult = recoilConfig?.Value ?? Vector2.zero;
            }
            else
            {
                WeaponHelper.CurrentRecoilMult = Vector2.one;
            }

            WeaponHelper.CurrentFirearmController = firearmController;
            WeaponHelper.IsPistolCurrentlyEquipped = isPistol;
            WeaponHelper.CurrentTemplate = template;
            WeaponHelper.CurrentRecoilVals = recoilVals;
        }
    }
}
