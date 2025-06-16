using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class ProceduralAnimationUpdatePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.method_8));
        }

        [PatchPostfix]
        private static void PatchPostfix(ProceduralWeaponAnimation __instance)
        {
            ShotEffector shotEffector = __instance.Shootingg;
            NewRecoilShotEffect newRecoil = shotEffector.NewShotRecoil;
            RecoilProcessBase camAngRecoil = newRecoil.CameraRotationRecoil;
            NewRotationRecoilProcess handAngRecoil = newRecoil.HandRotationRecoil;
            RecoilProcessBase handPosRecoil = newRecoil.HandPositionRecoil;
            Player.FirearmController firearmController = newRecoil._firearmController;
            Weapon weapon = firearmController?.Item;
            WeaponTemplate template = weapon?.Template;

            __instance.CrankRecoil = Plugin.EnableCrankRecoil.Value;

            __instance.CameraSmoothRecoil = Plugin.CameraSnap.Value;
            __instance.CameraToWeaponAngleSpeedRange = Plugin.CameraToWeaponAngleSpeed.Value;

            camAngRecoil.Intensity = Plugin.CameraRecoilIntensity.Value;
            camAngRecoil.ReturnSpeed = Plugin.CameraRecoilReturnSpeed.Value;
            camAngRecoil.Damping = Plugin.CameraRecoilDamping.Value;

            handAngRecoil.Intensity = Plugin.HandRecoilAngIntensity.Value;
            handAngRecoil.ReturnSpeed = Plugin.HandRecoilAngReturnSpeed.Value;
            handAngRecoil.Damping = Plugin.HandRecoilAngDamping.Value;

            handPosRecoil.Intensity = Plugin.HandRecoilPosIntensity.Value;
            handPosRecoil.ReturnSpeed = Plugin.HandRecoilPosReturnSpeed.Value;
            handPosRecoil.Damping = Plugin.HandRecoilPosDamping.Value;
        }
    }
}
