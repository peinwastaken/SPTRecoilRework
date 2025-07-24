using HarmonyLib;
using PeinRecoilRework.Config.Settings;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class WeaponRecoilInitializePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(WeaponRecoil), nameof(WeaponRecoil.Initialize));
        }

        [PatchPostfix]
        public static void PatchPostfix(WeaponRecoil __instance)
        {
            bool isPistol = WeaponHelper.IsPistolCurrentlyEquipped;
            DebugLogger.LogInfo(isPistol.ToString());
            float recoilRotationMult = isPistol ? PistolWeaponRecoilSettings.Multiplier.Value : WeaponRecoilSettings.Multiplier.Value;
            float recoilAimingMult = isPistol ? PistolWeaponRecoilSettings.AimingMultiplier.Value : WeaponRecoilSettings.AimingMultiplier.Value;
            float recoilTimeMult = isPistol ? PistolWeaponRecoilSettings.TimeMultiplier.Value : WeaponRecoilSettings.TimeMultiplier.Value;

            WeaponRecoilValue[] weaponRecoils = __instance.Values;
            foreach (WeaponRecoilValue recoil in weaponRecoils)
            {
                if (recoil.Process.ComponentType == ComponentType.Y)
                {
                    recoil.Process.CurveAimingValueMultiply *= recoilAimingMult;
                    recoil.Process.CurveValueMultiply *= recoilRotationMult;
                    recoil.Process.CurveTimeMultiply *= recoilTimeMult;
                }
            }
        }
    }
}
