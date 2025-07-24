using HarmonyLib;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class ShiftWeaponRootPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(PlayerBones), nameof(PlayerBones.ShiftWeaponRoot));
        }

        [PatchPrefix]
        private static bool PatchPrefix(PlayerBones __instance, ref bool isAiming)
        {
            isAiming = false; // temporary!

            return true;
        }
    }
}
