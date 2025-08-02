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
            WeaponRecoilData customData = WeaponHelper.FindRecoilData(weaponId);
            bool isPistol = WeaponHelper.IsPistol(template);
            
            CameraOffsetComponent cameraOffset = player?.GetComponent<CameraOffsetComponent>();

            if (player == null || player != Util.GetLocalPlayer())
            {
                return;
            }

            ConfigEntry<Vector2> recoilConfig;
            WeaponHelper.RealRecoilMultipliers.TryGetValue(weaponClass, out recoilConfig);
            WeaponHelper.CurrentRecoilMult = recoilConfig?.Value ?? Vector2.zero;

            float AngReturnSpeed = isPistol ? PistolRecoilAngSettings.PistolRecoilAngReturnSpeed.Value : RecoilAngSettings.RecoilAngReturnSpeed.Value;
            float AngDamping = isPistol ? PistolRecoilAngSettings.PistolRecoilAngDamping.Value : RecoilAngSettings.RecoilAngDamping.Value;

            float PosIntensity = isPistol ? PistolRecoilPosSettings.PistolRecoilPosIntensity.Value : RecoilPosSettings.RecoilPosIntensity.Value;
            float PosReturnSpeed = isPistol ? PistolRecoilPosSettings.PistolRecoilPosReturnSpeed.Value : RecoilPosSettings.RecoilPosReturnSpeed.Value;
            float PosDamping = isPistol ? PistolRecoilPosSettings.PistolRecoilPosDamping.Value : RecoilPosSettings.RecoilPosDamping.Value;

            float cameraSnap = isPistol ? GeneralSettings.PistolCameraSnap.Value : GeneralSettings.CameraSnap.Value;

            pwa.CrankRecoil = GeneralSettings.EnableCrankRecoil.Value;

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

                pwa.CameraSmoothRecoil = customData.OverrideProperties.CameraSnap ?? cameraSnap;
            }
            else
            {
                AngRecoil.ReturnSpeed = AngReturnSpeed;
                AngRecoil.Damping = AngDamping;

                PosRecoil.Intensity = PosIntensity;
                PosRecoil.ReturnSpeed = PosReturnSpeed;
                PosRecoil.Damping = PosDamping;

                pwa.CameraSmoothRecoil = cameraSnap;
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

            WeaponHelper.CurrentFirearmController = firearmController;
            WeaponHelper.IsPistolCurrentlyEquipped = isPistol;
            WeaponHelper.CurrentTemplate = template;
        }
    }
}
