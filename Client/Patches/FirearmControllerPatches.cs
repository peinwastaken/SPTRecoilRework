using Comfort.Common;
using EFT;
using HarmonyLib;
using PeinRecoilRework.Components;
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
            Player player = __instance.GetComponent<Player>();
            LeftStanceComponent lsc = player.GetComponent<LeftStanceComponent>();

            if (lsc != null && lsc.IsLeftStance)
            {
                origin += -__instance.WeaponRoot.right * 0.2f;
            }

            return true;
        }
    }
}