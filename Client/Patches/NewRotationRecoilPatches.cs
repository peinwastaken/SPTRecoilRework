using EFT.Animations;
using HarmonyLib;
using SPT.Reflection.Patching;
using SPTRecoilRework.Config.Settings;
using System.Reflection;

namespace SPTRecoilRework.Patches
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
