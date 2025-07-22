using EFT;
using HarmonyLib;
using PeinRecoilRework.Components;
using SPT.Reflection.Patching;
using System.Reflection;

namespace PeinRecoilRework.Patches
{
    public class ToggleLeftStancePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.ChangeLeftStance));
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player.FirearmController __instance)
        {
            Player player = __instance.GetComponent<Player>();
            LeftStanceComponent lsc = player.gameObject.GetComponent<LeftStanceComponent>();

            if (!player.IsYourPlayer || lsc == null)
            {
                return true;
            }
            else
            {
                if (__instance.Blindfire || player.MovementContext.IsInMountedState)
                {
                    return false;
                }

                lsc.SetLeftStance(!lsc.IsLeftStance);
            }

            return false;
        }
    }
}
