using EFT;
using HarmonyLib;
using PeinRecoilRework.Components;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class PlayerInitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.Init));
        }

        [PatchPostfix]
        public static void PatchPostfix(Player __instance)
        {
            if (__instance.IsYourPlayer == true)
            {
                __instance.gameObject.AddComponent<RealRecoilComponent>();
            }
        }
    }
}
