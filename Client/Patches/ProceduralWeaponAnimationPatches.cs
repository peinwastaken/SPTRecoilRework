using EFT;
using EFT.Animations;
using HarmonyLib;
using PeinRecoilRework.Components;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace PeinRecoilRework.Patches
{
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
        private static void PatchPostfix(ProceduralWeaponAnimation __instance, float str)
        {
            DebugLogger.LogInfo($"recoil strength: {str}");

            float scaleVert = 0.01f;
            float scaleHor = 0.01f;

            ShotEffector shotEffector = __instance.Shootingg;
            Player.FirearmController fc = shotEffector._firearmController;
            Player player = fc.gameObject.GetComponent<Player>();

            RealRecoilComponent realRecoil = player.gameObject.GetComponent<RealRecoilComponent>();
            if (realRecoil == null) return;

            CameraOffsetComponent cameraShake = player.gameObject.GetComponent<CameraOffsetComponent>();
            if (realRecoil == null) return;

            Vector2? realRecoilDirection = null;

            if (RealRecoilSettings.EnableRealRecoil.Value == true)
            {
                bool isMounted = __instance.IsMountedState || __instance.IsBipodUsed || __instance.IsVerticalMounting;
                bool isAiming = __instance.IsAiming;

                Vector2 weaponRecoil = WeaponHelper.GetWeaponRecoilValues(fc);

                float globalVerticalMult = RealRecoilSettings.RealRecoilVerticalMult.Value;
                float globalHorizontalMult = RealRecoilSettings.RealRecoilHorizontalMult.Value;
                float verticalMult = WeaponHelper.CurrentRecoilMult.y;
                float horizontalMult = WeaponHelper.CurrentRecoilMult.x;
                float mountedMult = isMounted ? RealRecoilSettings.RealRecoilMountedMult.Value : 1f;
                float aimingMult = isAiming ? RealRecoilSettings.RealRecoilAimingMult.Value : 1f;
                float stanceMult = Util.GetStanceMultiplier(player.Pose);

                DebugLogger.LogInfo($"vertical mult: {verticalMult}, horizontal mult: {horizontalMult}");

                float recoilVertical = weaponRecoil.y * scaleVert * stanceMult * mountedMult * aimingMult * verticalMult * globalVerticalMult;
                float recoilHorizontal = weaponRecoil.x * scaleHor * stanceMult * mountedMult * aimingMult * horizontalMult * globalHorizontalMult;

                realRecoilDirection = realRecoil.ApplyRecoil(recoilVertical, recoilHorizontal);
            }

            if (AdditionalCameraRecoilSettings.EnableAdditionalCameraRecoil.Value == true)
            {
                bool isUsingIrons = WeaponHelper.IsUsingIrons(__instance);
                float intensity = isUsingIrons ? 0f : 1f;

                DebugLogger.LogInfo($"isUsingIrons: {isUsingIrons}");

                if (realRecoilDirection == null)
                {
                    cameraShake.DoRecoilShake(null, intensity);
                }
                else
                {
                    cameraShake.DoRecoilShake(realRecoilDirection, intensity);
                }
            }
        }
    }
}
