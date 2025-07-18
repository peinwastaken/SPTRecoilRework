using EFT.Animations;
using HarmonyLib;
using PeinRecoilRework.Config.Settings;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class SetStableModePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRotationRecoilProcess), nameof(NewRotationRecoilProcess.SetStableMode));
        }

        [PatchPrefix]
        private static bool PatchPrefix(NewRotationRecoilProcess __instance, bool enable)
        {
            if (GeneralSettings.AllowStableRecoil.Value == false)
            {
                __instance.StableOn = false;
                return false;
            }

            return true;
        }
    }
}
