using EFT;
using HarmonyLib;
using PeinRecoilRework.Components;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

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
                __instance.gameObject.AddComponent<CameraOffsetComponent>();
            }
        }
    }
}
