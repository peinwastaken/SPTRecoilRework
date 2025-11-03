using HarmonyLib;
using SPT.Reflection.Patching;
using SPTRecoilRework.Config.Settings;
using SPTRecoilRework.Helpers;
using System.Reflection;

namespace SPTRecoilRework.Patches
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
            DebugLogger.LogInfo($"is current wpn pistol: {isPistol}");

            float recoilRotationMult = isPistol ? PistolRecoilSettings.RollMultiplier.Value : RecoilSettings.RollMultiplier.Value;
            float recoilAimingMult = isPistol ? PistolRecoilSettings.RollAimingMultiplier.Value : RecoilSettings.RollAimingMultiplier.Value;
            float recoilTimeMult = isPistol ? PistolRecoilSettings.RollTimeMultiplier.Value : RecoilSettings.RollTimeMultiplier.Value;

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
