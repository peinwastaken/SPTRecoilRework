using EFT;
using HarmonyLib;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class ToggleLeftStancePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LeftStanceController), nameof(LeftStanceController.ToggleLeftStance));
        }

        [PatchPrefix]
        private static bool PatchPrefix(LeftStanceController __instance)
        {
            WeaponHelper.IsLeftStance = !WeaponHelper.IsLeftStance;
            return false;
        }
    }
}
