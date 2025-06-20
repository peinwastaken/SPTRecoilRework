using EFT;
using HarmonyLib;
using PeinRecoilRework.Helpers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace PeinRecoilRework.Patches
{
    public class WeaponOverlapLeftStancePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.method_11));
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player.FirearmController __instance, ref Vector3 origin)
        {
            if (WeaponHelper.IsLeftStance)
            {
                origin += -__instance.WeaponRoot.right * 0.2f;
            }

            return true;
        }
    }
}